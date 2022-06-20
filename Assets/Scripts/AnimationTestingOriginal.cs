using UnityEngine;
using System.Collections;

public class AnimationTestingOriginal : MonoBehaviour
{
    public GameObject goDoc1;
    public GameObject goDoc2;
    public GameObject goDoc2HandLeft;
    public GameObject goPatient;
    public GameObject goBVM;
    public GameObject goBed;
    public GameObject goSittingProps;

    Animation animDoc1;
    Animation animDoc2;
    Animation animPatient;
    Animation animBVM;
    Animation animBed;

    // Use this for initialization
    void Start ()
    {
        animDoc1 = goDoc1.GetComponent<Animation>();
        animDoc2 = goDoc2.GetComponent<Animation>();
        animPatient = goPatient.GetComponent<Animation>();
        animBVM = goBVM.GetComponent<Animation>();
        animBed = goBed.GetComponent<Animation>();

        // Link BVM to doctors hand
        goBVM.transform.SetParent(goDoc2HandLeft.transform);

        // Play sitting animation
        StartCoroutine(SitIdle());
    }

    public void PlaySequence(string anim)
    {
        StopAllCoroutines();

        switch (anim)
        {
            case "CPR":
                StartCoroutine(CPR(30, 2));
                break;

            case "SitIdle":
                StartCoroutine(SitIdle());
                break;

            case "Defib":
                StartCoroutine(Defib());
                break;

            case "JawThrust":
                StartCoroutine(JawThrust());
                break;

            case "HeadTilt":
                StartCoroutine(HeadTilt());
                break;

            default:
                Debug.LogError("No animation sequence with name : " + anim);
                break;
        }
    }
	
	IEnumerator CPR(int compressionLoops, int bvmLoops)
    {
        // Set model visibility
        goDoc1.SetActive(true);
        goDoc2.SetActive(true);
        goSittingProps.SetActive(false);
        goBVM.SetActive(true);

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        while (true)
        {
            for (int i = 0; i < compressionLoops; i++)
            {
                animDoc1.Play("D1_CPR_CC_Loop");
                animPatient.Play("P_CPR_CC_Loop");

                while (animDoc1.isPlaying || animPatient.isPlaying)
                {
                    yield return null;
                }
            }

            animDoc1.Play("D1_CPR_CC_Pause");

            while (animDoc1.isPlaying)
            {
                yield return null;
            }

            for (int i = 0; i < bvmLoops; i++)
            {
                animDoc2.Play("D2_BVM");
                animPatient.Play("P_BVM");
                animBVM.Play("BVM_Compress");

                while (animDoc1.isPlaying || animPatient.isPlaying)
                {
                    yield return null;
                }
            }

            animDoc1.Play("D1_CPR_CC_Resume");

            while (animDoc1.isPlaying)
            {
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator Defib()
    {
        // Set model visibility
        goDoc1.SetActive(true);
        goDoc2.SetActive(true);
        goSittingProps.SetActive(false);
        goBVM.SetActive(true);

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        animDoc1.Play("D1_CPR_CC_Pause");
        animDoc2.Play("D2_BVM_Pause");
        animPatient.Play("P_Lying_Pose");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        animPatient.Play("P_Defib");

        while (animPatient.isPlaying)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        animDoc1.Play("D1_CPR_CC_Resume");
        animDoc2.Play("D2_BVM_Resume");

        while (animDoc1.isPlaying || animDoc2.isPlaying)
        {
            yield return null;
        }
    }

    IEnumerator SitIdle()
    {
        // Set model visibility
        goDoc1.SetActive(false);
        goDoc2.SetActive(false);
        goSittingProps.SetActive(true);

        // Set bed position
        animBed.Play("Bed_SitPos");

        // Run animation sequence
        while (true)
        {
            animPatient.Play("P_Sit_Idle_30bmp");

            while (animPatient.isPlaying)
            {
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator JawThrust()
    {
        // Set model visibility
        goDoc1.SetActive(false);
        goDoc2.SetActive(true);
        goSittingProps.SetActive(false);
        goBVM.SetActive(false);

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        while (true)
        {
            animPatient.Play("P_JawThrust");
            animDoc2.Play("D2_JawThrust");

            while (animPatient.isPlaying)
            {
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator HeadTilt()
    {
        // Set model visibility
        goDoc1.SetActive(true);
        goDoc2.SetActive(false);
        goSittingProps.SetActive(false);

        // Set bed position
        animBed.Play("Bed_LyingPos");

        // Run animation sequence
        while (true)
        {
            animPatient.Play("P_HeadTilt");
            animDoc1.Play("D1_HeadTilt");

            while (animPatient.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(1.0f);

            yield return null;
        }
    }
}
