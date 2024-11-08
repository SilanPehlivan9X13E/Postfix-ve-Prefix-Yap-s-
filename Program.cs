using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ödev_5
{
    public class Node //Düğüm sınıfı oluşturdum.
    {
        public char data; // Char, yani tek bir kelime alan veri yapısı
        public Node next; // Bir sonraki düğümü gösterir

        public Node(char data)
        {
            this.data = data;
            this.next = null; // İlk düğümde bir sonraki düğümü yok, boş 
        }
    }

    public class Liste //LinkedList yapısı için oluşturduğum Liste sınıfı
    {
        private Node head; // Liste baş düğümünü gösterir

        public void Ekle(char data)
        {
            Node eleman = new Node(data); // Yeni düğüm oluşturdum
            if (head == null)
            {
                head = eleman; // Listeyi başlat
            }
            else
            {
                Node temp = head; // Geçici düğüm oluşturarak ondan sonraki düğüme ekleme işlemi yapılır.
                while (temp.next != null)
                {
                    temp = temp.next; // Son düğüme git
                }
                temp.next = eleman; // Yeni düğümü ekleme
            }
        }

        public string PostfixeDonustur(string infix) // Infix ifadeyi postfix'e dönüştürme metodu
        {
            Stack<char> yıgın = new Stack<char>(); // Operatörleri tutmak için yazılan kod satırındaki yığın 
            string cıktı = ""; // Postfix ifadesi

            foreach (char a in infix)
            {
                if (char.IsLetterOrDigit(a)) // Harf veya rakam kontrolü
                //İfadenin harf (letter) ya da rakam(digit) olup olmadığını kontrol eder.
                {
                    cıktı += a; // Sonuna ekleme işlemi için
                }
                else if (a == '(') // Parantez açma
                {
                    yıgın.Push(a); // Yığına ekleme yaparız
                }
                else if (a == ')') // Parantez kapama
                {
                    while (yıgın.Count > 0 && yıgın.Peek() != '(')
                    {
                        cıktı += yıgın.Pop(); // Yığından eleman al
                    }
                    if (yıgın.Count > 0) // Açık parantez varsa yapıdan çıkarır.
                    {
                        yıgın.Pop(); // Parantezi çıkart
                    }
                }
                else // Operatör
                {
                    while (yıgın.Count > 0 && OncelikSırası(a) <= OncelikSırası(yıgın.Peek()))
                    {
                        cıktı += yıgın.Pop(); // Yığından eleman al
                    }
                    yıgın.Push(a); // Yığına operatör ekleme işlemi için
                }
            }
            while (yıgın.Count > 0)
            {
                cıktı += yıgın.Pop(); // Yığındaki tüm operatörleri almasını sağladım.
            }
            return cıktı; // Postfix ifadeyi döndür dedim
        }

        private int OncelikSırası(char o) // Operatör önceliğini belirtmek için switch case yapısı ile kontrol ettiğim matematiksel operatörler
        {
            switch (o)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                case '^':
                    return 3;
                default:
                    return 0;
            }
        }

        public int PostfixOlarakHesapla(string postfix)
        {
            Stack<int> yıgın = new Stack<int>(); // Sayıları tutmak için oluşturulan yığın kod satırı

            foreach (char a in postfix)
            {
                if (char.IsDigit(a)) // Rakam kontrolü yaptım
                {
                    yıgın.Push(a - '0'); // Rakamı yığına eklemek içiçn
                }
                else // Matematiksel operatörler için
                {
                    int y = yıgın.Pop(); // İlk sayı
                    int x = yıgın.Pop(); // İkinci sayı

                    switch (a)
                    {
                        case '+':
                            yıgın.Push(x + y); break; // Toplama işlemi
                        case '-':
                            yıgın.Push(x - y); break; // Çıkarma işlemi
                        case '*':
                            yıgın.Push(x * y); break; // Çarpma işlemi
                        case '/':
                            yıgın.Push(x / y); break; // Bölme işlemi
                    }
                }
            }
            return yıgın.Pop(); // Sonucu döndür
        }

        public string PrefixDonustur(string infix) // Prefix ifadeye dönüştür
        {
            // Infix ifadeyi ters çevirip işlem yapıyoruz
            char[] karakterDizisi = infix.ToCharArray();
            Array.Reverse(karakterDizisi);
            string tersInfix = new string(karakterDizisi);

            // Ters çevrilmiş infix ifadedeki parantezleri değiştir.
            //Bu kkod satırı yazılmaz ise prefix ifadesi yanlış hesaplarım.
            tersInfix = tersInfix.Replace('(', 'X').Replace(')', '(').Replace('X', ')'); //Bu kısma dikkat et!!!

            string postfix = PostfixeDonustur(tersInfix); // Ters infix'i postfix'e çevir

            // Postfix ifadeyi ters çevirerek prefix ifadesini al
            char[] postfixDizisi = postfix.ToCharArray();
            Array.Reverse(postfixDizisi);
            return new string(postfixDizisi); // Prefix ifadeyi döndür

            // Infix ifadeyi ters çevirip işlem yapıyoruz
            //            // Operatörlerin öncelik sırasına dikkat!!
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Liste baglantılıListe = new Liste(); // Liste nesnesi oluşturdum

            string infixIfadesi = "A+(B*C)/D"; // Hesaplanacak infix ifadesi

            string postfix = baglantılıListe.PostfixeDonustur(infixIfadesi); // Postfix ifadesini al
            Console.WriteLine("Postfix: " + postfix); // Postfix ifadesini yazdırır

            string prefix = baglantılıListe.PrefixDonustur(infixIfadesi); // Prefix ifadesini al
            Console.WriteLine("Prefix: " + prefix); // Prefix ifadesini yazdırır

            // Yazılan ifadeyi sayılarla ifade et
            string hesaplananPostfix = postfix.Replace("A", "1").Replace("B", "2").Replace("C", "3").Replace("D", "4"); // Replace ile değiştir
            int sonuc = baglantılıListe.PostfixOlarakHesapla(hesaplananPostfix); // Postfix'i hesaplama işlemi yaptım.
            Console.WriteLine("Sonuç: " + sonuc); // Sonucu yazdırma işlemi için

            Console.Read(); // Konsolu açık tut dedim
        }
    }
}
