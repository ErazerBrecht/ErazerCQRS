using System;

namespace Erazer.Domain.Files
{
    // TODO Entity interface
    public class File : IEquatable<File>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Type { get; }
        public int Size { get; set; }
        public long Created { get;}

        public File(Guid id, string name, string type, int size, long created)
        {
            if (Equals(id, default(Guid)))
                throw new ArgumentException("The Id cannot be the default value", nameof(id));

            Id = id;
            Name = name;
            Type = type;
            Size = size;
            Created = created;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var item = obj as File;
            return Equals(item);

        }

        public bool Equals(File item)
        {
            return item != null && Id == item.Id;
        }
    }
}
