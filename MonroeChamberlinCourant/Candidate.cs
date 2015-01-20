namespace MonroeChamberlinCourant.Framework
{
    public class Candidate
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Candidate(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
