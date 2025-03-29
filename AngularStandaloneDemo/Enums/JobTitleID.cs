using System.ComponentModel;

namespace AngularStandaloneDemo.Enums
{
    public enum JobTitleID

    {
        [Description("Administrator")]
        Admin = 0,

        [Description("Doctor")]
        Doctor = 1,

        [Description("Nurse")]
        Nurse = 2,

        [Description("Management Staff")]
        Management = 3,
    }
}
