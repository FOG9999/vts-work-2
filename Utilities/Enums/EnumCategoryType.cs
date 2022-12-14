namespace Utilities.Enums
{
    /// <summary>
    /// Enum category of type
    /// </summary>
    /// Description: Biểu diễn các loại danh mục
    public enum enCategoryType
    {
        [StringValue("Tỉnh thành")]
        Province = 1,

        [StringValue("Vị trí")]
        LevelDoctor = 2,

        [StringValue("Chức danh")]
        Position = 3,

        [StringValue("H.Hàm/H.Vị")]
        EducationIndex = 4,

        [StringValue("Loại ngày nghỉ")]
        TypeOfHolidays = 5,
    }
   
}
