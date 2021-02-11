using DocStore.Data.Interfaces;
using DocStore.Data.Models;
using PDFService.Controllers;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace NUnitTestProjectPDF
{
    public class Tests
    {
  
        [Test]
        public void TestDownloadFile()
        {

            // arrange
            // create mock
            var mockDocRepo = new Mock<IDocRepository>();



            // Need to pass a binary value constructer otherwise it will fail when trying to dowload the file
            byte[] content = Encoding.ASCII.GetBytes("0x255044462D312E320A25E2E3CFD30A322030206F626A3C3C2F526563745B302030203020305D2F502033203020522F46542F5369672F46203133322F537562747970652F5769646765742F562031203020522F547970652F416E6E6F742F5428BEDEC74730155166B055293E3E0A656E646F626A0A312030206F626A3C3C2F4C");
         
            mockDocRepo.Setup(x => x.GetDoc(@"C:\Users\adils\source\repos\PDFService\PDFService\202000215104.pdf")).Returns(new Doc() {Id = 4, Name = "202000215104.pdf", Data = content });

            var docsController = new DocsController(mockDocRepo.Object);



            // Act
            var result = docsController.GetDoc(@"C:\Users\adils\source\repos\PDFService\PDFService\202000215104.pdf");


            // Assert

            HttpResponseMessage response = new HttpResponseMessage();

            // set response to OK
            response.StatusCode = System.Net.HttpStatusCode.OK;

            // set to BadRequest
            //response.StatusCode = System.Net.HttpStatusCode.BadRequest;

            Assert.AreEqual(response.StatusCode,result.Value.StatusCode);
        }

        [Test]
        public void TestDeleteFile()
        {

            // arrange
            // create mock
            var mockDocRepo = new Mock<IDocRepository>();

            mockDocRepo.Setup(x => x.RemoveDoc(@"C:\Users\adils\source\repos\PDFService\PDFService\202000215104.pdf")).Returns(true);

            var docsController = new DocsController(mockDocRepo.Object);



            // Act

             var result = docsController.DeleteDoc(@"C:\Users\adils\source\repos\PDFService\PDFService\202000215104.pdf");


            // Assert
            // it should return null as the provided location should be deleted by then.
            Assert.AreEqual(null, result.Value);
        }
    }
}