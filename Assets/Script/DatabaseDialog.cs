using UnityEngine;
//loww
[System.Serializable]
public class DataPertanyaan
{
    [Header("Pertanyaan NPC")]
    [TextArea(2, 4)]
    public string pertanyaan;

    [Header("Kunci Jawaban")]
    public bool jawabanBenar;

    [Header("Respon Rubah")]
    [TextArea(2, 4)]
    public string jawabanRubahBenar;

    [TextArea(2, 4)]
    public string jawabanRubahSalah;
}

public class DatabaseDialog : MonoBehaviour
{
    [Header("Daftar Pertanyaan Otomatis")]
    public DataPertanyaan[] daftarPertanyaan;

    private void Awake()
    {
        // Jika belum diisi dari Inspector, otomatis isi 10 pertanyaan
        if (daftarPertanyaan == null || daftarPertanyaan.Length == 0)
        {
            daftarPertanyaan = new DataPertanyaan[10];

            daftarPertanyaan[0] = new DataPertanyaan
            {
                pertanyaan = "Apakah rubah termasuk hewan mamalia?",
                jawabanBenar = true,
                jawabanRubahBenar = "Benar! Rubah termasuk hewan mamalia.",
                jawabanRubahSalah = "Salah. Rubah termasuk mamalia."
            };

            daftarPertanyaan[1] = new DataPertanyaan
            {
                pertanyaan = "Apakah ikan bernapas menggunakan paru-paru?",
                jawabanBenar = false,
                jawabanRubahBenar = "Benar! Ikan bernapas menggunakan insang.",
                jawabanRubahSalah = "Salah. Ikan tidak bernapas dengan paru-paru."
            };

            daftarPertanyaan[2] = new DataPertanyaan
            {
                pertanyaan = "Apakah matahari merupakan pusat tata surya?",
                jawabanBenar = true,
                jawabanRubahBenar = "Benar! Matahari adalah pusat tata surya.",
                jawabanRubahSalah = "Salah. Matahari adalah pusat tata surya."
            };

            daftarPertanyaan[3] = new DataPertanyaan
            {
                pertanyaan = "Apakah manusia memiliki tiga jantung?",
                jawabanBenar = false,
                jawabanRubahBenar = "Benar! Manusia hanya memiliki satu jantung.",
                jawabanRubahSalah = "Salah. Manusia tidak memiliki tiga jantung."
            };

            daftarPertanyaan[4] = new DataPertanyaan
            {
                pertanyaan = "Apakah tumbuhan memerlukan cahaya matahari untuk fotosintesis?",
                jawabanBenar = true,
                jawabanRubahBenar = "Benar! Fotosintesis membutuhkan cahaya matahari.",
                jawabanRubahSalah = "Salah. Tumbuhan memerlukan cahaya untuk fotosintesis."
            };

            daftarPertanyaan[5] = new DataPertanyaan
            {
                pertanyaan = "Apakah air laut terasa manis?",
                jawabanBenar = false,
                jawabanRubahBenar = "Benar! Air laut terasa asin.",
                jawabanRubahSalah = "Salah. Air laut tidak terasa manis."
            };

            daftarPertanyaan[6] = new DataPertanyaan
            {
                pertanyaan = "Apakah Indonesia berada di benua Asia?",
                jawabanBenar = true,
                jawabanRubahBenar = "Benar! Indonesia berada di benua Asia.",
                jawabanRubahSalah = "Salah. Indonesia berada di benua Asia."
            };

            daftarPertanyaan[7] = new DataPertanyaan
            {
                pertanyaan = "Apakah semua burung bisa terbang?",
                jawabanBenar = false,
                jawabanRubahBenar = "Benar! Tidak semua burung bisa terbang.",
                jawabanRubahSalah = "Salah. Ada burung yang tidak bisa terbang."
            };

            daftarPertanyaan[8] = new DataPertanyaan
            {
                pertanyaan = "Apakah komputer termasuk perangkat elektronik?",
                jawabanBenar = true,
                jawabanRubahBenar = "Benar! Komputer adalah perangkat elektronik.",
                jawabanRubahSalah = "Salah. Komputer termasuk perangkat elektronik."
            };

            daftarPertanyaan[9] = new DataPertanyaan
            {
                pertanyaan = "Apakah bulan memancarkan cahaya sendiri?",
                jawabanBenar = false,
                jawabanRubahBenar = "Benar! Cahaya bulan berasal dari pantulan matahari.",
                jawabanRubahSalah = "Salah. Bulan tidak memancarkan cahaya sendiri."
            };
        }
    }
}
