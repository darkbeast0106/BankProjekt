using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BankProjekt.Tests
{
    [TestFixture]
    class BankTest
    {
        Bank b;

        [SetUp]
        public void Setup()
        {
            b = new Bank();
        }

        [TestCase]
        public void UjSzamlaHibaNelkulLetrejon()
        {
            Assert.DoesNotThrow(() => b.UjSzamla("Teszt Elek", "1234"));
        }

        [TestCase]
        public void UjSzamlaDuplikaltSzamlaszam()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<ArgumentException>(() 
                => b.UjSzamla("Kovács Elek", "1234"));
        }

        [TestCase]
        public void UjSzamlaLetezoNevvelNincsHiba()
        {
            Assert.DoesNotThrow(() => b.UjSzamla("Teszt Elek", "1234"));
            Assert.DoesNotThrow(() => b.UjSzamla("Teszt Elek", "1235"));
        }

        [TestCase]
        public void EgyenlegFeltoltNemLetezoSzamlara()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(()
                => b.EgyenlegFeltolt("4321", 10000));
        }

        [TestCase]
        public void EgyenlegFeltoltLetezoSzamlara()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.DoesNotThrow(()
                => b.EgyenlegFeltolt("1234", 10000));
        }

        [TestCase]
        public void UjSzamlaEgyenlege0()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.AreEqual(0, b.Egyenleg("1234"));
        }

        [TestCase]
        public void EgyenlegNemLetezoSzamlaraExceptiontDob()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.Throws<HibasSzamlaszamException>(()
                => b.Egyenleg("4321"));
        }

        [TestCase]
        public void EgyenlegFeltoltesSikerül()
        {
            b.UjSzamla("Teszt Elek", "1234");
            Assert.AreEqual(0, b.Egyenleg("1234"));
            b.EgyenlegFeltolt("1234", 10000);
            Assert.AreEqual(10000, b.Egyenleg("1234"));
        }

        [TestCase]
        public void EgyenlegFeltoltMegfeleloSzamlaraMegy()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            Assert.AreEqual(0, b.Egyenleg("1234"));
            Assert.AreEqual(0, b.Egyenleg("5678"));
            b.EgyenlegFeltolt("1234", 10000);
            Assert.AreEqual(10000, b.Egyenleg("1234"));
            Assert.AreEqual(0, b.Egyenleg("5678"));
        }
        [TestCase]
        public void Utal0Forint()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.Throws<ArgumentException>(() 
                => b.Utal("1234", "5678", 0));
        }

        [TestCase]
        public void UtalNincsElegPenz()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.IsFalse(b.Utal("1234", "5678", 20000));
            Assert.AreEqual(10000, b.Egyenleg("1234"));
            Assert.AreEqual(0, b.Egyenleg("5678"));
        }

        [TestCase]
        public void UtalMindenSikeres()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.IsTrue(b.Utal("1234", "5678", 3000));
            Assert.AreEqual(7000, b.Egyenleg("1234"));
            Assert.AreEqual(3000, b.Egyenleg("5678"));
        }
        [TestCase]
        public void OnmaganakUtal()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.Throws<ArgumentException>(()
                => b.Utal("1234", "1234", 3000));
        }

        [TestCase]
        public void UtalNemLetezoSzamlara()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.Throws<HibasSzamlaszamException>(()
                => b.Utal("1234", "4321", 3000));
        }
        [TestCase]
        public void UtalNemLetezoSzamlarol()
        {
            b.UjSzamla("Teszt Elek", "1234");
            b.UjSzamla("Nagy Árpád", "5678");
            b.EgyenlegFeltolt("1234", 10000);
            Assert.Throws<HibasSzamlaszamException>(()
                => b.Utal("4321", "5678", 3000));
        }

    }
}
