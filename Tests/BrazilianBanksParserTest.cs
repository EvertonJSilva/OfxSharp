using NUnit.Framework;
using NUnit.Framework.Legacy;
using OfxSharpLib;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OFXSharp.Tests
{
    [TestFixture]
    public class BrazilianBanksParserTest
    {

        [Test]
        public void CanParseItauBR_ComDecimaisComVirgula()
        {
            var parser = new OfxDocumentParser();
            var ofxDocument = parser.Import(new FileStream(@"itau_padrao_BR.ofx", FileMode.Open));

            Assert.AreEqual(ofxDocument.Account.AccountId, "9999 99999-9");
            Assert.AreEqual(ofxDocument.Account.BankId, "0341");

            Assert.AreEqual(3, ofxDocument.Transactions.Count());

            foreach (var transaction in ofxDocument.Transactions)
            {             
                if (transaction.Memo == "RSHOP")
                    Assert.AreEqual(transaction.Amount, -666.66);
                if (transaction.Memo == "SISDEB")
                    Assert.AreEqual(transaction.Amount, -77.77);
                if (transaction.Memo == "REND PAGO APLIC AUT MAIS")
                    Assert.AreEqual(transaction.Amount, 99.99);
            }

            CollectionAssert.AreEqual(ofxDocument.Transactions.Select(x => x.Memo.Trim()).ToList(), new[] { "RSHOP", "REND PAGO APLIC AUT MAIS", "SISDEB" });
        }

        [Test]
        public void CanParseItau()
        {
            var parser = new OfxDocumentParser();
            var ofxDocument = parser.Import(new FileStream(@"itau.ofx", FileMode.Open));

            Assert.AreEqual(ofxDocument.Account.AccountId, "9999 99999-9");
            Assert.AreEqual(ofxDocument.Account.BankId, "0341");

            Assert.AreEqual(3, ofxDocument.Transactions.Count());
            CollectionAssert.AreEqual(ofxDocument.Transactions.Select(x => x.Memo.Trim()).ToList(), new[] { "RSHOP", "REND PAGO APLIC AUT MAIS", "SISDEB" });
        }

        [Test]
        public void CanParseSantander()
        {
            var parser = new OfxDocumentParser();
            var ofxDocument = parser.Import(new FileStream(@"santander.ofx", FileMode.Open));

            Assert.IsNotNull(ofxDocument);
        }

        [Test]
        public void CanParseBancoDoBrasil()
        {
            var parser = new OfxDocumentParser();
            var ofxDocument = parser.Import(new FileStream(@"bb.ofx", FileMode.Open), Encoding.GetEncoding("ISO-8859-1"));

            Assert.AreEqual(ofxDocument.Account.AccountId, "99999-9");
            Assert.AreEqual(ofxDocument.Account.BranchId, "9999-9");
            Assert.AreEqual(ofxDocument.Account.BankId, "1");

            Assert.AreEqual(3, ofxDocument.Transactions.Count());
            CollectionAssert.AreEqual(ofxDocument.Transactions.Select(x => x.Memo.Trim()).ToList(), new[] { "Transferência Agendada", "Compra com Cartão", "Saque" });
            
            Assert.IsNotNull(ofxDocument);
        }
    }
}
