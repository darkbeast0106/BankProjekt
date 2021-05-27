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
    }
}
