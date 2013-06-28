namespace EntityFrameworkTest.Model
{
    public class ArrangementVersion : BusinessObject
    {
        public int Version { get; set; }
        public VersionStatus Status { get; set; }

        // Navigation Properties
        public Arrangement Arrangement { get; set; }
    }
}
