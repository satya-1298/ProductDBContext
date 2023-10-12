using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Xml;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepoLayer.Entity
{
    public class ProductEntity
    {
        //     ProductId INT IDENTITY(1,1) PRIMARY KEY,
        //Code VARCHAR(50) UNIQUE NOT NULL,         
        // Name VARCHAR(250) NOT NULL,
        // Description VARCHAR(4000),              
        // ExpiryDate DATE CHECK(ExpiryDate >= GETDATE()), 
        // Category VARCHAR(50) ,
        // Image NVARCHAR(MAX),                    
        // Status VARCHAR(10) DEFAULT 'Active',      
        // CreationDate DATETIME DEFAULT GETDATE()
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }

    }
}
