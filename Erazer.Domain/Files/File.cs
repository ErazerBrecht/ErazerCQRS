using System;
using System.Collections.Generic;
using System.Text;

namespace Erazer.Domain.Files
{
    // TODO Entity interface
    public class File : IEquatable<File>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Type { get; }
        public int Size { get; set; }
        public DateTime Created { get;}
        public Guid UserId { get; }

        public File(Guid id, string name, string type, int size, DateTime created, Guid userId)
        {
            if (Equals(id, default(Guid)))
                throw new ArgumentException("The Id cannot be the default value", nameof(id));

            Id = id;
            Name = name;
            Type = type;
            Size = size;
            Created = created;
            UserId = userId;
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
            if (item == null)
            {
                return false;
            }

            return Id == item.Id;
        }
    }
}
