using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// Hàm thư viện để nén file JS và css
    /// </summary>
	public class JSCompressor
	{
        private bool removeLineFeeds, compressVarNames, varCompTest;
        private char[] newVarName;
        private int varNamePos;
        private StringCollection scNoComps;
        private StringCollection scLiterals;
        private Regex reInsLit, reExtNoComp, reDelNoComp, reFuncParams,
                reFindVars, reStripVarPrefix, reStripParens, reStripAssign;
        private MatchEvaluator meExtNoComp, meInsLit;
        private int literalCount, noCompCount;
        public bool LineFeedRemoval
        {
            get { return removeLineFeeds; }
            set { removeLineFeeds = value; }
        }
        public bool CompressVariableNames
        {
            get { return compressVarNames; }
            set { compressVarNames = value; }
        }
        public bool TestVariableNameCompression
        {
            get { return varCompTest; }
            set { varCompTest = value; }
        }
        public JSCompressor() : this(true)
        {
        }
        public JSCompressor(bool removeLFs)
        {
            removeLineFeeds = removeLFs;
            scLiterals = new StringCollection();
            scNoComps = new StringCollection();
            compressVarNames = true;
        }
     
        public JSCompressor(bool removeLFs, bool compressVars) : this(removeLFs)
        {
            compressVarNames = compressVars;
        }       
        public string Compress(string strScript)
        {
            string strCompressed;
            char [] achScriptChars;
            if(strScript == null || strScript.Length == 0)
                return strScript;
            scLiterals.Clear();
            scNoComps.Clear();
            if(reInsLit == null)
            {
                reExtNoComp = new Regex(@"//\s*#pragma\s*NoCompStart.*?" +
                    @"//\s*#pragma\s*NoCompEnd.*?\n",
                    RegexOptions.Multiline | RegexOptions.Singleline |
                    RegexOptions.IgnoreCase);
                reDelNoComp = new Regex(@"//\s*#pragma\s*NoComp(Start|End).*\n",
                    RegexOptions.Multiline | RegexOptions.IgnoreCase);
                reInsLit = new Regex("\xFE|\xFF");
                meInsLit = new MatchEvaluator(OnMarkerFound);
                meExtNoComp = new MatchEvaluator(OnNoCompFound);

                reFuncParams = new Regex(@"function.*?\((.*?)\)(.*?|\n)?\{",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);
                reFindVars = new Regex(@"(var\s+.*?)(;|$)",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);
                reStripVarPrefix = new Regex(@"^var\s+",
                    RegexOptions.IgnoreCase);
                reStripParens = new Regex(@"\(.*?,.*?\)|\[.*?,.*?\]",
                    RegexOptions.IgnoreCase);
                reStripAssign = new Regex(@"(=.*?)(,|;|$)",
                    RegexOptions.IgnoreCase);
            }
            strCompressed = reExtNoComp.Replace(strScript, meExtNoComp);
            achScriptChars = strCompressed.ToCharArray();
            CompressArray(achScriptChars);
            strCompressed = new String(achScriptChars);
            strCompressed = strCompressed.Replace("\0", String.Empty);
            if(!varCompTest)
            {
                strCompressed = Regex.Replace(strCompressed, @"^[\s]+|[ \f\r\t\v]+$",
                    String.Empty, RegexOptions.Multiline);
                strCompressed = Regex.Replace(strCompressed, @"([\s]){2,}", "$1");
                if(removeLineFeeds)
                {
                    strCompressed = Regex.Replace(strCompressed, @"([+-])\n\1",
                        "$1 $1");
                    strCompressed = Regex.Replace(strCompressed, @"([^+-][+-])\n",
                        "$1");
                    strCompressed = Regex.Replace(strCompressed,
                        @"([\xFE{}([,<>/*%&|^!~?:=.;])\n", "$1");
                    strCompressed = Regex.Replace(strCompressed,
                        @"\n([{}()[\],<>/*%&|^!~?:=.;+-])" ,"$1");
                }
                strCompressed = Regex.Replace(strCompressed,
                    @"[ \f\r\t\v]?([\n\xFE\xFF/{}()[\];,<>*%&|^!~?:=])[ \f\r\t\v]?",
                    "$1");
                strCompressed = Regex.Replace(strCompressed, @"([^+]) ?(\+)",
                    "$1$2");
                strCompressed = Regex.Replace(strCompressed, @"(\+) ?([^+])",
                    "$1$2");
                strCompressed = Regex.Replace(strCompressed, @"([^-]) ?(\-)",
                    "$1$2");
                strCompressed = Regex.Replace(strCompressed, @"(\-) ?([^-])",
                    "$1$2");
                if(removeLineFeeds)
                {
                    strCompressed = Regex.Replace(strCompressed,
                        @"(\W(if|while|for)\([^{]*?\))\n", "$1");
                    strCompressed = Regex.Replace(strCompressed,
                        @"(\W(if|while|for)\([^{]*?\))((if|while|for)\([^{]*?\))\n",
                        "$1$3");
                    strCompressed = Regex.Replace(strCompressed,
                        @"([;}]else)\n", "$1 ");
                }
            }
            if(compressVarNames || varCompTest)
                strCompressed = CompressVariables(strCompressed);
            noCompCount = literalCount = 0;
            strCompressed = reInsLit.Replace(strCompressed, meInsLit);

            return strCompressed;
        }
        private void ExtractLiteral(char [] achScriptChars, int nStartPos,
            int nEndPos)
        {
            int nLen = nEndPos - nStartPos + 1;

            scLiterals.Add(new String(achScriptChars, nStartPos, nLen));

            achScriptChars[nStartPos] = '\xFF';

            Array.Clear(achScriptChars, nStartPos + 1, nLen - 1);
        }
        private static bool IsRegExpStart(char [] achScriptChars, int nCurPos)
        {
            char ch;

            while(nCurPos-- > 0)
            {
                ch = achScriptChars[nCurPos];
                if(Char.IsWhiteSpace(ch) == false)
                    return (ch == '(' || ch == ';' || ch == '=') ? true : false;
            }

            return true;
        }
        private void CompressArray(char [] achScriptChars)
        {
            bool bInComment = false;
            int  nIdx, nLen = achScriptChars.Length, nStartPos = -1;
            char chCur, chNext, chEnd = '\0';

            for(nIdx = 0; nIdx < nLen; nIdx++)
            {
                chCur = achScriptChars[nIdx];
                if(nStartPos > -1)
                {
                    if(bInComment == true)
                    {
                        if(chEnd == '*')
                        {
                            if(nIdx - nStartPos > 2 &&
                              achScriptChars[nIdx - 1] == '*' && chCur == '/')
                            {
                                Array.Clear(achScriptChars, nStartPos,
                                    nIdx - nStartPos + 1);
                                nStartPos = -1;
                                bInComment = false;
                            }
                        }
                        else
                            if(chCur == '\r' || chCur == '\n')
                            {
                                Array.Clear(achScriptChars, nStartPos,
                                    nIdx - nStartPos + 1);
                                nStartPos = -1;
                                bInComment = false;
                            }
                    }
                    else    
                        if(chCur == chEnd)
                        {
                            ExtractLiteral(achScriptChars, nStartPos, nIdx);
                            nStartPos = -1;
                        }
                        else   
                            if(chCur == '\\')
                                nIdx++;
                }
                else
                    if(nIdx < nLen - 1)
                    {
                        
                        if(chCur == '/')
                        {
                            chNext = achScriptChars[nIdx + 1];

                            if(chNext == '*' || chNext == '/')
                            {
                                nStartPos = nIdx++;
                                chEnd = chNext;
                                if(nIdx < nLen - 1 && chNext == '*' &&
                                  achScriptChars[nIdx + 1] == '@')
                                    nStartPos = -1;
                                else
                                    bInComment = true;
                            }
                            else
                                if(JSCompressor.IsRegExpStart(achScriptChars, nIdx))
                                {
                                    nStartPos = nIdx;
                                    chEnd = chCur;
                                }
                        }
                        else
                            if(chCur == '\'' || chCur == '\"')
                            {
                                chEnd = chCur;
                                nStartPos = nIdx;
                            }
                            else 
                                if(chCur == '\r' && !varCompTest)
                                    achScriptChars[nIdx] = '\n';
                    }
            }
        }
        private string OnMarkerFound(Match match)
        {
            if(match.Value == "\xFE")
                return scNoComps[noCompCount++];

            return scLiterals[literalCount++];
        }
        private string OnNoCompFound(Match match)
        {
            scNoComps.Add(reDelNoComp.Replace(match.Value, String.Empty));
            return "\xFE";
        }
        private string CompressVariables(string script)
        {
            StringCollection scVariables = new StringCollection();
            string[] varNames;
            string name = null, matchName;
            bool incVarName;
            MatchCollection matches = reFuncParams.Matches(script);

            foreach(Match m in matches)
            {
                varNames = m.Groups[1].Value.Split(',');
                foreach(string s in varNames)
                {
                    name = s.Trim();

                    if(name.Length != 0 && !scVariables.Contains(name))
                        scVariables.Add(name);
                }
            }
            matches = reFindVars.Matches(script);

            foreach(Match m in matches)
            {
                name = reStripVarPrefix.Replace(m.Groups[1].Value, String.Empty);
                name = reStripParens.Replace(name, String.Empty);
                name = reStripAssign.Replace(name, "$2");
                varNames = name.Split(',');
                foreach(string s in varNames)
                {
                    name = s.Trim();

                    if(name.Length != 0 && !scVariables.Contains(name))
                        scVariables.Add(name);
                }
            }
            newVarName = new char[10];
            newVarName[0] = '\x60';
            varNamePos = 0;
            incVarName = true;
            foreach(string replaceName in scVariables)
            {
                if(incVarName)
                {
                    do
                    {
                        IncrementVariableName();

                        name = new String(newVarName, 0, varNamePos + 1);
                        matchName = @"\W" + name + @"\W";

                    } while(Regex.IsMatch(script, matchName));

                    incVarName = false;
                }
                if(name.Length < replaceName.Length)
                {
                    incVarName = true;
                    script = Regex.Replace(script,
                        @"(\W)" + replaceName + @"(?=\W)", "$1" + name);
                }
            }

            return script;
        }
        private void IncrementVariableName()
        {
            if(newVarName[varNamePos] != 'z')
                newVarName[varNamePos]++;
            else
            {
                if(varNamePos == 0)
                {
                    newVarName[0] = '_';
                    varNamePos++;
                }
                else
                {
                    // _a to _z, _aa to _az, _ba to _bz, etc
                    if(newVarName[varNamePos - 1] == '_' ||
                      newVarName[varNamePos - 1] == 'z')
                    {
                        if(newVarName[varNamePos - 1] == '_')
                            newVarName[varNamePos] = 'a';
                        else
                            newVarName[varNamePos - 1] = 'a';

                        varNamePos++;
                    }
                    else
                        newVarName[varNamePos - 1]++;
                }

                newVarName[varNamePos] = 'a';
            }
        }
    }
}
