namespace EntityFrameworkTest.Model
{
    public class ArrangementVersion : BusinessObject
    {
        public virtual int Version { get; set; }
        public virtual VersionStatus Status { get; set; }   // no workie EF < 5.0

        // Navigation Properties
        public virtual Arrangement Arrangement { get; set; }
        //public virtual int ArrangementId { get; set; }

        public ArrangementVersion()
        {
        }
    }
}
