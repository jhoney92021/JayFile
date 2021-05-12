using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FunWithFiles.Models
{
    public class FileDataModel
    {
        [Required]
        [Column(name:"FileId")]
        [Key]
        int FileId {get;set;}
        [Required]
        [Column(name:"File")]
        public List<FileDataRowViewModel> File {get;set;}
        [Required]
        [Column(name:"FileExtension")]
        string FileExtension {get;set;}
        [Required]
        [Column(name:"FileName")]
        string FileName {get;set;}
    }
}