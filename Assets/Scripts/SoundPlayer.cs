using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SoundPlayer : MonoBehaviour
{
    public Hub hub;

    public GameObject defibChargingSound;
    public GameObject defibChargedSound;
    public GameObject CPRSound;
    public GameObject ShockSound;
    public GameObject VentSound;

    private AudioSource defibChargingSource;
    private AudioSource defibChargedSource;
    private AudioSource cPRSound;
    private AudioSource shockSound;
    private AudioSource ventSound;
    public AudioSource beepSound;
    public AudioSource cardiacArrestSound;
    public AudioSource noPulse01;
    public AudioSource noPulse02;
    public AudioSource chargingDefibSpeech;
    public AudioSource deliveringShock;
    public AudioSource shockDelivered;
    public AudioSource dumpingShock;
    public AudioSource nonShockableRhythm;

	public bool speechEnabled = false;

    private List<AudioSource> sourceList;

    public bool stopSounds = false;

    // Use this for initialization
    void Start()
    {
        defibChargingSource = defibChargingSound.GetComponent<AudioSource>();
        defibChargedSource = defibChargedSound.GetComponent<AudioSource>();
        cPRSound = CPRSound.GetComponent<AudioSource>();
        shockSound = ShockSound.GetComponent<AudioSource>();
        ventSound = VentSound.GetComponent<AudioSource>();

        sourceList = new List<AudioSource>();
        sourceList.Add(defibChargingSource);
        sourceList.Add(defibChargedSource);
        sourceList.Add(cPRSound);
        sourceList.Add(shockSound);
        sourceList.Add(ventSound);
        sourceList.Add(beepSound);
        sourceList.Add(cardiacArrestSound);
        sourceList.Add(noPulse01);
        sourceList.Add(noPulse02);
        sourceList.Add(chargingDefibSpeech);
        sourceList.Add(deliveringShock);
        sourceList.Add(shockDelivered);
        sourceList.Add(dumpingShock);
        sourceList.Add(nonShockableRhythm);

        cPRSound.volume = 0.5f;
        ventSound.volume = 0.5f;
        beepSound.volume = 0.1f;
        beepSound.pitch = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string sound)
    {
        //StopAllCoroutines();

        switch (sound)
        {
            case "DefibCharging":
                StartCoroutine(DefibCharging());
                break;

            case "DefibCharged":
                StartCoroutine(DefibCharged());
                break;

            case "CPR":
                StartCoroutine(CPR());
                break;

            case "Shock":
                StartCoroutine(Shock());
                break;

            case "Vent":
                StartCoroutine(Vent());
                break;

            case "Beep":
                StartCoroutine(Beep());
                break;

            case "CardiacArrestCall":
                StartCoroutine(CardiacArrestCall());
                break;

            case "NoPulse01":
                StartCoroutine(NoPulse01());
                break;

            case "NoPulse02":
                StartCoroutine(NoPulse02());
                break;

            case "DeliveringShock":
                StartCoroutine(DeliveringShock());
                break;

            case "ShockDelivered":
                StartCoroutine(ShockDelivered());
                break;

            case "DumpingShock":
                StartCoroutine(DumpingShock());
                break;

            case "NonShockableRhythm":
                StartCoroutine(NonShockableRhythm());
                break;

            default:
                Debug.LogError("No sound with name : " + sound);
                break;
        }
    }

    IEnumerator DefibCharging()
    {
        StopAllSounds();
        stopSounds = false;

		if (hub.cardiacArrest && speechEnabled) {
			chargingDefibSpeech.Play ();
		}
        defibChargingSource.Play();

        while (defibChargingSource.isPlaying || chargingDefibSpeech.isPlaying)
        {
            yield return null;
        }
        if (!stopSounds)
        {
            PlaySound("DefibCharged");
        }
        else
        {
            stopSounds = false;
        }
    }

    IEnumerator DefibCharged()
    {
		defibChargedSource.Play ();
        while (defibChargedSource.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator CPR()
    {
        cPRSound.Play();
        while (cPRSound.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator Shock()
    {
        shockSound.Play();
        while (shockSound.time<0.2f)
        {
            yield return null;
        }

        defibChargedSource.Stop();

        while (shockSound.isPlaying) {
            yield return null;
        }
    }

    IEnumerator Vent()
    {
        ventSound.Play();

        while (ventSound.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator Beep()
    {
        beepSound.Play();

        while (beepSound.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator CardiacArrestCall()
    {
        cardiacArrestSound.Play();

        while (cardiacArrestSound.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        hub.CardiacArrestCallFinished();
    }

    IEnumerator NoPulse01()
    {
		if (speechEnabled) {
			StopAllSounds ();
			noPulse01.Play ();

			while (noPulse01.isPlaying) {
				yield return null;
			}
		} else {
			yield return null;
		}
    }

    IEnumerator NoPulse02()
    {
		if (speechEnabled) {
			StopAllSounds ();
			noPulse02.Play ();

			while (noPulse02.isPlaying) {
				yield return null;
			}
		} else {
			yield return null;
		}
    }

    IEnumerator DeliveringShock()
    {
		if (speechEnabled) {
			deliveringShock.Play ();
			while (deliveringShock.isPlaying) {
				yield return null;
			}
			hub.PlaySequence("Defib");
		} else {
			hub.PlaySequence("Defib");
			yield return null;
		}
    }

    IEnumerator DumpingShock()
    {
		if (speechEnabled) {
			StopAllSounds ();

			yield return new WaitForSeconds (0.1f);

			dumpingShock.Play ();

			while (dumpingShock.isPlaying) {
				yield return null;
			}
		} else {
			yield return null;
		}
    }

    IEnumerator NonShockableRhythm()
    {
        StopAllSounds();

        yield return new WaitForSeconds(0.5f);

		if (speechEnabled) {
			nonShockableRhythm.Play ();

			hub.SendMessage ("\"That's a non-shockable rhythm, resume CPR.\"", 0, 2, false);

			while (nonShockableRhythm.isPlaying) {
				yield return null;
			}
		}
    }

    IEnumerator ShockDelivered()
    {
		if (speechEnabled) {
			shockDelivered.Play ();

			while (shockDelivered.isPlaying) {
				yield return null;
			}
		} else {
			yield return null;
		}
    }

    public void StopAllSounds()
    {
        for (int i = sourceList.Count - 1; i >= 0; --i)
        {
            sourceList[i].Stop();
            stopSounds = true;
        }
    }

}
