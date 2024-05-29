using UnityEngine;
using System.Collections;

public class ControlTest : MonoBehaviour
{

    public float heartRateUpper = 150f;
    public float heartRateLower = 60f;
    public float heartRate = 80f;
    private float duration;
    private Vector3 bending = new Vector3(0f, 0.35f, 0f);
    private Vector3 startPosition;
    private Vector3 endPosition;
    //These are the Vector3 co-ordinates of the screen edges
    private Vector3 screenLeft;
    private Vector3 screenRight;
    //These are the x co-ordinates of the screen edges
    private float scrnLft;
    private float scrnRt;
    private float lastScreenWidth;
    private float screenWidth;
    private float traceRate;
    private float startTime;
    private float timeStamp;
    private string wave;
    private float cycles = 0f;
    private float QTc;
    //FOR CALCULATING DISTANCE BETWEEN BEATS
    private float lengthy;
    private float gap;
    public GameObject spriteRen;
    public GameObject defibScreen;
    private string genClone = "GenericSprite(Clone)";
    private string desClone = "GenericSprite";
    public Insights rhythm = Insights.HeartRhythmNSR;
    public bool variableRhythm = true;
    public bool wideQRS = false;
    private float pWidth = 0.08f;
    private float pqWidth = 0.1f;
    private float qrWidth = 0.02f;
    private float rsWidth = 0.02f;
    private float stwWidth = 0.02f;
    private float stsWidth;
    private float tWidth;
    public float scale = 1f;
    private bool kill = false;
    public float stripLength = 5f;
    private float pDur = 0.01f;
    private float localY = 0f;

    // Use this for initialization
    void Start()
    {
        bending = new Vector3(bending.x, (bending.y * scale), bending.z);
        Instantiate(spriteRen, transform.position,
            Quaternion.identity);
        spriteRen = GameObject.Find(genClone);
        spriteRen.transform.parent = defibScreen.transform;

        SetRhythm();

        //endPosition will be start position for 1st cycle
        spriteRen.transform.localPosition = new Vector3(-0.5f, 0f, -1);
        localY = spriteRen.transform.position.y;
        spriteRen.transform.position = new Vector3(spriteRen.transform.position.x,
            spriteRen.transform.position.y, defibScreen.transform.position.z - 0.01f);
        endPosition = spriteRen.transform.position;
        scrnLft = endPosition.x;
        screenWidth = defibScreen.GetComponent<Renderer>().bounds.size.x;
        traceRate = screenWidth / 5;

        scale = defibScreen.GetComponent<Renderer>().bounds.size.y / 15f;

        SetOtherFactors();

        startTime = Time.time;
        STsegment();
    }

    void SetRhythm()
    {
        if (rhythm == Insights.HeartRhythmNSR)
        {
            //COMMENTED OUT SECTIONS BELOW ARE FOR USER-SET FACTORS
            //CURRENT SCRIPT IS FOR RANDOMLY CHANGING RHYTHM
            /*if (heartRate < 20f)
            {
                heartRate = 20f;
            }
            if (heartRate > 150f)
            {
                heartRate = 150f;
            }*/
            heartRate = Random.Range(20f, 150f);

            wideQRS = !wideQRS;

            variableRhythm = false;
        }
        else if (rhythm == "af")
        {
            heartRate = Random.Range(20f, 150f);
            heartRateUpper = Random.Range(heartRate + (heartRate / 10), heartRate + (heartRate / 3));
            heartRateLower = Random.Range(heartRate - (heartRate / 10), heartRate - (heartRate / 3));
            wideQRS = false;
        }
        else if (rhythm == Insights.HeartRhythmAtrialFlutter)
        {
            heartRate = 75f;
            bool[] trueFalse = new bool[] { true, false };
            int randomN = Random.Range(0, 2);
            variableRhythm = trueFalse[randomN];

            if (variableRhythm)
            {
                wideQRS = false;
            }
        }
        else if (rhythm == Insights.HeartRhythmSVT)
        {
            /*if (heartRate < 120f)
            {
                heartRate = 120f;
            }*/
            heartRate = Random.Range(120f, 300f);
            variableRhythm = false;
            wideQRS = false;
        }
        else if (rhythm == Insights.HeartRhythmVT || rhythm == Insights.HeartRhythmVF)
        {
            wideQRS = true;
            variableRhythm = false;
            heartRate = 300f;
        }
        /*if (heartRateUpper > 300f)
        {
            heartRateUpper = 300f;
        }
        if (heartRateUpper < heartRateLower)
        {
            heartRateLower = 60f;
            heartRateUpper = 100f;
        }*/
    }

    void SetOtherFactors()
    {
        if (wideQRS)
        {
            stwWidth = 0.12f;
        }
        else if (rhythm == Insights.HeartRhythmSVT)
        {
            stwWidth = 0.08f;
        }
        else
        {
            stwWidth = 0.02f;
        }

        //SET QTc (THIS WILL GIVE HALF QTc VALUE
        QTc = ((((300 * Mathf.Sqrt(60f / heartRate)) - 40f) / 2f) / 1000);

        if (rhythm == "af")
        {
            heartRate = Random.Range(heartRateLower, heartRateUpper);
            pqWidth = (60f / heartRate) * 0.08f;
        }

        //NB: BELOW TECHNIQUE WON'T WORK FOR AF - RELIES ON FIXED LENGTH OF HEARTBEATS
        if (rhythm == Insights.HeartRhythmVT || rhythm == Insights.HeartRhythmVF)
        {
            lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + 0.015f;
        }
        else if (rhythm == Insights.HeartRhythmNSR)
        {
            lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
        }
        else
        {
            lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
        }
        gap = (60f / heartRate) - lengthy;
    }

    public void Change(/*string newRhyth*/)
    {
        int pos = Random.Range(0, 6);
        //Debug.Log("pos = " + pos);
        Insights[] rhythms = new string[] { "af", Insights.HeartRhythmAtrialFlutter, Insights.HeartRhythmNSR, Insights.HeartRhythmSVT, Insights.HeartRhythmVT, Insights.HeartRhythmVF };
        Insights newRhyth = rhythms[pos];
        Debug.Log("rhythm = " + rhythms[pos]);
        rhythm = newRhyth;
        kill = true;
        wave = "sts";
        SetRhythm();
        SetOtherFactors();
        Update();
    }

    public void Wide()
    {
        wideQRS = !wideQRS;
        if (!wideQRS)
        {
            stwWidth = 0.02f;
        }
        else
        {
            stwWidth = 0.12f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmpPos = spriteRen.transform.localPosition;
        if (tmpPos.x < 0.5)
        {
            if ((Time.time - timeStamp) <= duration)
            {
                Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, ((Time.time - timeStamp) / duration));
                if (wave == "p" || wave == "t")
                {
                    //currentPos.x += bending.x*Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/pDuration) * Mathf.PI);
                    currentPos.y += bending.y * Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp) / duration) * Mathf.PI);
                    //currentPos.z += bending.z*Mathf.Sin(Mathf.Clamp01((Time.time - timeStamp)/pDuration) * Mathf.PI);
                }
                if (rhythm == "af")
                {
                    if (wave == "tp")
                    {
                        float y = 0.05f * scale;
                        pDur = 0.01f;
                        float targetTime = (Time.time - timeStamp) / (pDur * traceRate);
                        if (targetTime > 2)
                        {
                            pDur = Random.Range(0.005f, 0.01f);
                            y = (Random.Range(0.01f, 0.05f)) * scale;
                            bending = new Vector3(0f, y, 0f);
                            //Debug.Log ("pDur: " + pDur + " y: " + y);
                        }
                        currentPos.y += bending.y * Mathf.Sin(((Time.time - timeStamp) / ((0.01f / scale) * traceRate)) * Mathf.PI);
                    }
                }
                else if (rhythm == Insights.HeartRhythmAtrialFlutter)
                {
                    if (wave == "tp")
                    {
                        bending = new Vector3(0f, 0.15f * scale, 0f);
                        currentPos.y += bending.y * Mathf.Sin(((Time.time - timeStamp) / ((0.02f / scale) * traceRate)) * Mathf.PI);
                        //Debug.Log(currentPos.y);
                    }
                }
                else if (rhythm == Insights.HeartRhythmVF)
                {
                    if (wave == "tp")
                    {
                        float y = (Random.Range(0.005f, 3f)) * scale;
                        bending = new Vector3(0f, y, 0f);
                        currentPos.y += bending.y * Mathf.Sin(((Time.time - timeStamp) / ((0.03f / scale) * traceRate)) * Mathf.PI);
                    }
                }
                spriteRen.transform.position = currentPos;
            }
            else {
                endPosition.x += (Time.time - timeStamp - duration) * traceRate;
                if (rhythm == "af" || rhythm == Insights.HeartRhythmAtrialFlutter)
                {
                    if (wave == "qr")
                    {
                        RS();
                    }
                    else if (wave == "rs")
                    {
                        STwave();
                    }
                    else if (wave == "stw")
                    {
                        STsegment();
                    }
                    else if (wave == "sts")
                    {
                        T();
                    }
                    else if (wave == "t")
                    {
                        TP();
                    }
                    else if (wave == "tp" || wave == "pq")
                    {
                        QR();
                    }
                }
                else if (rhythm == Insights.HeartRhythmVT)
                {
                    if (wave == "qr")
                    {
                        RS();
                    }
                    else if (wave == "rs")
                    {
                        STwave();
                    }
                    else if (wave == "stw" || wave == "sts")
                    {
                        T();
                    }
                    else if (wave == "t")
                    {
                        TP();
                    }
                    else if (wave == "tp" || wave == "pq")
                    {
                        QR();
                    }
                }
                else if (rhythm == Insights.HeartRhythmVF)
                {
                    TP();
                }
                else if (rhythm == Insights.HeartRhythmNSR)
                {
                    if (wave == "p")
                    {
                        PQ();
                    }
                    else if (wave == "pq")
                    {
                        QR();
                    }
                    else if (wave == "qr")
                    {
                        RS();
                    }
                    else if (wave == "rs")
                    {
                        STwave();
                    }
                    else if (wave == "stw")
                    {
                        STsegment();
                    }
                    else if (wave == "sts")
                    {
                        T();
                    }
                    else if (wave == "t")
                    {
                        TP();
                    }
                    else if (wave == "tp")
                    {
                        P();
                    }
                }
                else if (rhythm == Insights.HeartRhythmSVT)
                {
                    if (wave == "qr")
                    {
                        RS();
                    }
                    else if (wave == "rs")
                    {
                        STwave();
                    }
                    else if (wave == "stw")
                    {
                        STsegment();
                    }
                    else if (wave == "sts")
                    {
                        T();
                    }
                    else if (wave == "t")
                    {
                        TP();
                    }
                    else if (wave == "tp" || wave == "pq")
                    {
                        QR();
                    }
                }
            }
        }
        else {
            Vector3 currentPos = new Vector3(scrnLft, spriteRen.transform.position.y, spriteRen.transform.position.z);
            endPosition.x -= screenWidth;
            startPosition.x -= screenWidth;
            Instantiate(spriteRen, currentPos,
                Quaternion.identity);

            GameObject isDes = GameObject.Find(desClone);
            if (isDes)
            {
                Destroy(isDes);
            }
            desClone += "(Clone)";
            genClone += "(Clone)";

            spriteRen = GameObject.Find(genClone);
            spriteRen.transform.parent = defibScreen.transform;

            Update();
        }
    }

    void SetVariables(string wav, float dur)
    {
        //Debug.Log("Wave" + wav);
        startPosition = endPosition;
        spriteRen.transform.position = startPosition;
        wave = wav;
        duration = dur;
        timeStamp = Time.time;
        if (wave == "qr")
        {
            if (wideQRS)
            {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), (1f * scale), startPosition.z);
            }
            else {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), (4f * scale), startPosition.z);
            }
        }
        else if (wave == "rs")
        {
            if (wideQRS)
            {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), (-4f * scale), startPosition.z);
            }
            else {
                endPosition = new Vector3((startPosition.x + (duration * traceRate)), (-1.5f * scale), startPosition.z);
            }
        }
        else {
            endPosition = new Vector3((startPosition.x + (duration * traceRate)), 0f, startPosition.z);
        }
        Update();
    }

    void P()
    {
        SetVariables("p", pWidth);
    }

    void PQ()
    {
        SetVariables("pq", pqWidth);
    }

    void QR()
    {
        bending = new Vector3(0f, (0.35f * scale), 0f);
        SetVariables("qr", qrWidth);
    }

    void RS()
    {
        SetVariables("rs", rsWidth);
    }

    void STwave()
    {
        SetVariables("stw", stwWidth);
    }

    void STsegment()
    {
        if (rhythm == Insights.HeartRhythmVT)
        {
            SetVariables("sts", 0.01f);
        }
        else if (rhythm == Insights.HeartRhythmVF)
        {
            //VF is just the TP segment on loop, but first call on instantiation is to STsegment(), so this is a redirect:
            TP();
        }
        else {
            SetVariables("sts", 0.5F * QTc);
        }
    }

    void T()
    {
        if (rhythm == Insights.HeartRhythmVT || rhythm == Insights.HeartRhythmVF)
        {
            bending = new Vector3(0f, (1.2f * scale), 0f);
            SetVariables("t", 0.15f);
        }
        else {
            SetVariables("t", 1.5F * QTc);
        }
    }

    void TP()
    {
        if (rhythm == Insights.HeartRhythmAtrialFlutter)
        {
            if (variableRhythm)
            {
                int var1 = Random.Range(0, 5);
                float var2 = (float)var1;
                if (var2 == 0f)
                {
                    QR();
                }
                else {
                    SetVariables("tp", (var2 / 2) * gap);
                }
            }
            else {
                SetVariables("tp", gap);
            }
        }
        else if (rhythm == "af")
        {
            heartRate = Random.Range(heartRateLower, heartRateUpper);
            pqWidth = (60f / heartRate) * 0.08f;
            QTc = ((((300 * Mathf.Sqrt(60f / heartRate)) - 40f) / 2f) / 1000);
            lengthy = (pWidth + pqWidth + qrWidth + rsWidth + stwWidth) + (QTc * 2);
            gap = (60f / heartRate) - lengthy;
            bending = new Vector3(0f, (0.03f * scale), 0f);
            SetVariables("tp", gap);
        }
        else if (rhythm == Insights.HeartRhythmVF)
        {
            float y = (Random.Range(0.01f, 1f)) * scale;
            pDur = Random.Range(0.005f, 0.01f);
            bending = new Vector3(0f, y, 0f);
            SetVariables("tp", 0.5f);
        }
        else if (rhythm == Insights.HeartRhythmSVT)
        {
            if (gap <= 0)
            {
                QR();
            }
            else {
                SetVariables("tp", gap);
            }
        }
        else {
            SetVariables("tp", gap);
        }
    }
}