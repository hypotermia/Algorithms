using System;
using System.Collections.Generic;
using System.Linq;

public class Tagihan
{
    public string Nama { get; set; }
    public DateTime JatuhTempo { get; set; }
    public decimal Jumlah { get; set; }
    public decimal Dibayar { get; set; } = 0;

}
public class TagihanPayment
{
    public string NoTagihan { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalTagihan { get; set; }
    public List<Pembayaran> PembayaranList { get; set; } = new List<Pembayaran>();
}
public class Pembayaran
{
    public string NoPayment { get; set; }
    public string NoTagihan { get; set; }
    public DateTime TanggalBayar { get; set; }
    public decimal JumlahBayar { get; set; }
}

public class PenaltyResult
{
    public string NoTagihan { get; set; }
    public int NoPenalty { get; set; }
    public decimal TagihanOverdue { get; set; }
    public int HariKeterlambatan { get; set; }
    public decimal AmountPenalty { get; set; }
}

public class Program
{
    public static void AlokasiPembayaran(List<Tagihan> tagihanList, decimal nominalPayment)
    {
        if (nominalPayment < 0)
        {
            Console.WriteLine("Nominal payment tidak boleh kurang dari 0.");
            return;
        }

        var sisaPayment = nominalPayment;

        var sortedTagihan = tagihanList.OrderBy(t => t.JatuhTempo).ToList();

        foreach (var tagihan in sortedTagihan)
        {
            if (sisaPayment <= 0)
                break;

            if (sisaPayment >= tagihan.Jumlah)
            {
                tagihan.Dibayar = tagihan.Jumlah;
                sisaPayment -= tagihan.Jumlah;
            }
            else
            {
                tagihan.Dibayar = sisaPayment;
                sisaPayment = 0;
            }
        }

        Console.WriteLine($"Input Payment = {nominalPayment}");
        foreach (var tagihan in sortedTagihan)
        {
            Console.WriteLine($"{tagihan.Nama}\tDue: {tagihan.JatuhTempo:dd MMM yy}\t{tagihan.Jumlah:N0}\t{tagihan.Dibayar:N0}");
        }

        if (sisaPayment > 0)
        {
            Console.WriteLine($"Sisa pembayaran yang belum teralokasi: {sisaPayment:N0}");
        }
    }
    public static List<PenaltyResult> HitungPenalty(List<TagihanPayment> tagihanList, List<Pembayaran> pembayaranList, DateTime today)
    {
        var penaltyResults = new List<PenaltyResult>();

        foreach (var tagihan in tagihanList)
        {
            decimal remainingAmount = tagihan.TotalTagihan;
            DateTime lastPaidDate = tagihan.DueDate;


            var pembayaranTagihan = pembayaranList
                .Where(p => p.NoTagihan == tagihan.NoTagihan)
                .OrderBy(p => p.TanggalBayar)
                .ToList();

            foreach (var pembayaran in pembayaranTagihan)
            {
                if (remainingAmount <= 0)
                    break;


                var bayarUntukTagihan = Math.Min(remainingAmount, pembayaran.JumlahBayar);
                remainingAmount -= bayarUntukTagihan;


                DateTime effectiveDate = pembayaran.TanggalBayar > tagihan.DueDate ? pembayaran.TanggalBayar : tagihan.DueDate;
                int lateDays = (effectiveDate - lastPaidDate).Days;

                if (lateDays > 0)
                {
                    penaltyResults.Add(new PenaltyResult
                    {
                        NoTagihan = tagihan.NoTagihan,
                        NoPenalty = penaltyResults.Count(p => p.NoTagihan == tagihan.NoTagihan) + 1,
                        TagihanOverdue = bayarUntukTagihan,
                        HariKeterlambatan = lateDays,
                        AmountPenalty = bayarUntukTagihan * (decimal)0.002 * lateDays
                    });
                }

                else
                {
                    penaltyResults.Add(new PenaltyResult
                    {
                        NoTagihan = tagihan.NoTagihan,
                        NoPenalty = penaltyResults.Count(p => p.NoTagihan == tagihan.NoTagihan) + 1,
                        TagihanOverdue = remainingAmount,
                        HariKeterlambatan = lateDays,
                        AmountPenalty = bayarUntukTagihan * (decimal)0.002 * lateDays
                    });
                }
                lastPaidDate = pembayaran.TanggalBayar;
            }


            if (remainingAmount > 0)
            {
                int lateDays = (today - lastPaidDate).Days;

                if (lateDays > 0)
                {
                    penaltyResults.Add(new PenaltyResult
                    {
                        NoTagihan = tagihan.NoTagihan,
                        NoPenalty = penaltyResults.Count(p => p.NoTagihan == tagihan.NoTagihan) + 1,
                        TagihanOverdue = remainingAmount,
                        HariKeterlambatan = lateDays,
                        AmountPenalty = remainingAmount * (decimal)0.002 * lateDays
                    });
                }
            }
        }

        return penaltyResults;
    }
    public static int CalculateScore(List<int> numbers)
    {
        int totalScore = 0;

        foreach (var num in numbers)
        {
            if (num == 8)
            {
                totalScore += 5;
            }
            else if (num % 2 == 0)
            {
                totalScore += 1;
            }
            else
            {
                totalScore += 3;
            }
        }

        return totalScore;
    }
    static void TampilkanPolaA(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= i; j++)
            {
                Console.Write(i);
            }
            Console.WriteLine();
        }
    }

    static void TampilkanPolaB(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            for (int j = i; j >= 1; j--)
            {
                Console.Write(j);
            }
            Console.WriteLine();
        }
    }

    static void TampilkanPolaC(int n)
    {
        int mulai = 1;
        int flagBatas = 0;
        int flagMulai = 1;
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= i; j++)
            {

                if(flagMulai == 1)
                {
                    Console.Write(mulai);
                    if (mulai < n)
                    {
                        mulai++;
                    }
                    else
                    {
                        mulai--;
                        flagMulai = 0;
                        flagBatas = 1;


                    }
                }
                else
                {

                    Console.Write(mulai);
                    if (mulai != 1)
                    {

                        mulai--;
                    }
                    else
                    {
                        flagMulai = 1;
                        mulai++;
                    }

                }





            }

            Console.WriteLine();
        }
    }
    static void TampilkanPolaD(int n)
    {
        int count = 1; 
        for (int i = 1; i <= n; i++) 
        {
            for (int j = 0; j < n; j++) 
            {
                Console.Write((count + j * n) + "\t"); 
            }
            count++; 
            Console.WriteLine();
        }
    }

    public static void Main()
    {
        var tagihanList = new List<Tagihan>
        {
            new Tagihan { Nama = "Tagihan#1", JatuhTempo = new DateTime(2023, 1, 10), Jumlah = 165000 },
            new Tagihan { Nama = "Tagihan#2", JatuhTempo = new DateTime(2023, 2, 15), Jumlah = 80000 },
            new Tagihan { Nama = "Tagihan#3", JatuhTempo = new DateTime(2023, 1, 20), Jumlah = 130000 },
            new Tagihan { Nama = "Tagihan#4", JatuhTempo = new DateTime(2023, 3, 25), Jumlah = 416000 },
            new Tagihan { Nama = "Tagihan#5", JatuhTempo = new DateTime(2023, 2, 10), Jumlah = 95500 },
            new Tagihan { Nama = "Tagihan#6", JatuhTempo = new DateTime(2023, 8, 17), Jumlah = 523000 }
        };
        var pembayaranList = new List<Pembayaran>
{
    new Pembayaran { NoPayment = "Payment#1", NoTagihan = "Tagihan#1", TanggalBayar = new DateTime(2023, 1, 10), JumlahBayar = 165000 },
    new Pembayaran { NoPayment = "Payment#2", NoTagihan = "Tagihan#3", TanggalBayar = new DateTime(2023, 2, 20), JumlahBayar = 130000 },
    new Pembayaran { NoPayment = "Payment#2", NoTagihan = "Tagihan#5", TanggalBayar = new DateTime(2023, 2, 20), JumlahBayar = 95500 },
    new Pembayaran { NoPayment = "Payment#3", NoTagihan = "Tagihan#2", TanggalBayar = new DateTime(2023, 2, 25), JumlahBayar = 30000 },
    new Pembayaran { NoPayment = "Payment#4", NoTagihan = "Tagihan#2", TanggalBayar = new DateTime(2023, 3, 30), JumlahBayar = 50000 },
    new Pembayaran { NoPayment = "Payment#4", NoTagihan = "Tagihan#4", TanggalBayar = new DateTime(2023, 3, 30), JumlahBayar = 50000 }
};
        var tagihanPembayaranList = new List<TagihanPayment> { 

            new TagihanPayment { NoTagihan = "Tagihan#1", DueDate = new DateTime(2023, 1, 10), TotalTagihan = 165000 },
            new TagihanPayment { NoTagihan = "Tagihan#2", DueDate = new DateTime(2023, 2, 15), TotalTagihan = 80000 },
            new TagihanPayment { NoTagihan = "Tagihan#3", DueDate = new DateTime(2023, 1, 20), TotalTagihan = 130000 },
            new TagihanPayment { NoTagihan = "Tagihan#4", DueDate = new DateTime(2023, 3, 30), TotalTagihan = 416000 },
            new TagihanPayment { NoTagihan = "Tagihan#5", DueDate = new DateTime(2023, 2, 10), TotalTagihan = 95500 }

        };

        Console.WriteLine("Choose one of 2 Type ");
        Console.WriteLine("1. Sorting And Alokasi ");
        Console.WriteLine("2. Perhitungan Penalty ");
        Console.WriteLine("3. Array Of Integers ");
        Console.WriteLine("4. Last Soal");
        int a = int.Parse(Console.ReadLine());
        if (a == 1)
        {
            Console.WriteLine("Masukkan nominal payment:");
            if (decimal.TryParse(Console.ReadLine(), out var nominalPayment))
            {
                AlokasiPembayaran(tagihanList, nominalPayment);
            }
            else
            {
                Console.WriteLine("Input payment tidak valid.");
            }

        }
        else if (a == 2)
        {
            DateTime today = new DateTime(2022, 4, 29);
            var penaltyResults = HitungPenalty(tagihanPembayaranList, pembayaranList, today);

            Console.WriteLine("No Tagihan\tNo Penalty\tTagihan Overdue\tHari Keterlambatan\tAmount Penalty");
            foreach (var result in penaltyResults)
            {
                Console.WriteLine($"{result.NoTagihan}\t{result.NoPenalty}\t{result.TagihanOverdue:N0}\t{result.HariKeterlambatan}\t{result.AmountPenalty:N0}");
            }
        }
        else if (a == 3)
        {
            Console.WriteLine("Masukkan angka-angka, pisahkan dengan coma:");
            string input = Console.ReadLine();


            List<int> numbers = input
                .Split(',', StringSplitOptions.RemoveEmptyEntries) 
                .Select(int.Parse) 
                .ToList();


            int score = CalculateScore(numbers);


            Console.WriteLine($"Output: {score}");
        }
        else if (a == 4)
        {
            Console.Write("Masukkan nilai n:");
            int n = int.Parse(Console.ReadLine());
            TampilkanPolaA(n);

            TampilkanPolaB(n);

            TampilkanPolaC(n);


            TampilkanPolaD(n);
        }


    }
}
