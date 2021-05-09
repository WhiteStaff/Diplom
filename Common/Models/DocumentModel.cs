using System;

namespace Models
{
    public class DocumentModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }
    }
}