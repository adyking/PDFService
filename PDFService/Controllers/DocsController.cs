using DocStore.Data.Interfaces;
using DocStore.Data.Models;
using DocStore.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;


namespace PDFService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : ControllerBase
    {

        private IDocRepository docs;

        public DocsController(IDocRepository _docs)
        {
            this.docs = _docs;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Doc>> GetAllDocs()
        {
            return docs.GetAllDocs();
        }

        [HttpGet]
        [Route("GetAllSortedDocs")]
        public ActionResult<IEnumerable<Doc>> GetAllSortedDocs()
        {
            return docs.GetAllSortedDocs();
        }


        [HttpGet("{location}")]
        public ActionResult<HttpResponseMessage> GetDoc(string location)
        {

            var doc = docs.GetDoc(location);

            if (doc == null)
                return NotFound();

            return DownloadFile(doc);


        }


        [HttpPost("upload", Name = "upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public  ActionResult<IFormFile> PostFile(IFormFile file)
        {
            if (CheckIfPdfFile(file))
            {
                if(file.Length > 5242880) // 5MB Max
                    return BadRequest(new { message = "File size has exceed 5MB" });

                return WriteFileToDatabase(file);
            }
            else
            {
                return BadRequest(new { message = "Invalid pdf file extension" });
            }

           
        }


        [HttpDelete("{location}")]
        public ActionResult<IEnumerable<Doc>> DeleteDoc(string location)
        {

            if (docs.RemoveDoc(location))
                return docs.GetAllDocs();
                

            return BadRequest(new { message = "Could not find the file to delete" });

        }

        private  ActionResult<IFormFile> WriteFileToDatabase(IFormFile file)
        {


            BinaryReader Br = new BinaryReader(file.OpenReadStream());
            Byte[] bytes = Br.ReadBytes((Int32)file.OpenReadStream().Length);


            string fileLocation = Path.GetFullPath(file.FileName);


            var doc = new Doc() {

                Name = file.FileName,
                Data = bytes,
                FileSize = file.Length.ToString(),
                Location = fileLocation


            };
         

            if (docs.AddNewDoc(doc))
                 return Ok();

            return BadRequest();

        }

        private bool CheckIfPdfFile(IFormFile file)
        {
            var fileExtension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

            return (fileExtension.ToLower() == ".pdf");
        }


        private HttpResponseMessage DownloadFile(Doc doc)
        {


            //Set Response.
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            //Set the Response Content.
            response.Content = new ByteArrayContent(doc.Data);

            //Set the Response Content Length.
            response.Content.Headers.ContentLength = doc.Data.LongLength;

            //Set the Content Disposition Header Value and FileName.
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = doc.Name;

            //Set the File Content Type.
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");


            return response;


        }


    }




}

