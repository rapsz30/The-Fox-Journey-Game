using UnityEngine;
using TMPro;
using UnityEngine.UI;
//loww
public class ManajerDialog : MonoBehaviour
{
    [Header("Database")]
    public DatabaseDialog database;

    [Header("Bubble")]
    public GameObject bubbleNPC;
    public GameObject bubbleRubah;

    [Header("Teks")]
    public TextMeshProUGUI teksPertanyaan;
    public TextMeshProUGUI teksJawabanRubah;
    public TextMeshProUGUI teksSkor;

    [Header("Tombol")]
    public Button tombolBenar;
    public Button tombolSalah;

    private int indeksSoal = 0;
    private int skor = 0;
    private bool sudahMenjawab = false;

    void Start()
    {
        Debug.Log("ManajerDialog START JALAN");
        tombolBenar.onClick.AddListener(() => Jawab(true));
        tombolSalah.onClick.AddListener(() => Jawab(false));

        TampilkanSoal();
    }

    void TampilkanSoal()
    {
        if (indeksSoal >= database.daftarPertanyaan.Length)
        {
            teksPertanyaan.text = "Semua pertanyaan selesai!";
            bubbleRubah.SetActive(false);
            tombolBenar.gameObject.SetActive(false);
            tombolSalah.gameObject.SetActive(false);
            return;
        }

        sudahMenjawab = false;

        bubbleNPC.SetActive(true);
        bubbleRubah.SetActive(false);

        DataPertanyaan soal = database.daftarPertanyaan[indeksSoal];
        teksPertanyaan.text = soal.pertanyaan;
        teksJawabanRubah.text = "";

        tombolBenar.interactable = true;
        tombolSalah.interactable = true;
    }

    void Jawab(bool jawabanPemain)
    {
        if (sudahMenjawab) return;
        sudahMenjawab = true;

        bubbleRubah.SetActive(true);

        DataPertanyaan soal = database.daftarPertanyaan[indeksSoal];

        if (jawabanPemain == soal.jawabanBenar)
        {
            teksJawabanRubah.text = soal.jawabanRubahBenar;
            skor += 10;
        }
        else
        {
            teksJawabanRubah.text = soal.jawabanRubahSalah;
        }

        teksSkor.text = "Skor: " + skor;

        // lanjut ke soal berikutnya setelah 2 detik
        Invoke(nameof(SoalBerikutnya), 2f);
    }

    void SoalBerikutnya()
    {
        indeksSoal++;
        TampilkanSoal();
    }
}
