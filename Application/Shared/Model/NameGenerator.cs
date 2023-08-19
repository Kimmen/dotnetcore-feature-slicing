namespace Kimmen.FeatureSlicing.Api.Web.Shared.Model
{
    internal static class NameGenerator
    {
        public static string FullName(this NamedTeacher teacher)
        {
            return $"{teacher.AddressedAs}. {teacher.LastName}"; 
        }

        public static string FullName(this NamedStudent student)
        {
            return $"{student.LastName}, {student.FirstName}";
        }
    }
}
