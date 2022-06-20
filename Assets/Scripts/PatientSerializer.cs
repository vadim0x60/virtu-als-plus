using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;


public class PatientSerializer : MonoBehaviour {
	private bool loaded = false;
    private string source = "";

	PatientContainer pC;

	public void Save(Patient p)
	{
		List<bool> boolers = new List <bool>();
		boolers.Add(p.arrest);
		boolers.Add(p.conscious);
		boolers.Add(p.airwayObstructed);
		boolers.Add(p.apnoeic);
		boolers.Add(p.pulmonaryOedema);
		boolers.Add (p.bradyCardia);

		pC.boolers = boolers;

		List<string> stringers = new List <string> ();
		stringers.Add (p.diagnosis);
		stringers.Add (p.leadIn);
		stringers.Add (p.rhythm1);
		stringers.Add (p.rhythm2);
		stringers.Add (p.rhythm3);
		stringers.Add (p.rhythm4);
		stringers.Add (p.rhythm5);
		stringers.Add (p.airwayObstruction);
		stringers.Add (p.chestFindings);
		stringers.Add (p.pupilsFindings);
		stringers.Add (p.AVPU);

		pC.stringers = stringers;

		List<float> floaters = new List <float> ();
		floaters.Add (p.heartRate1);
		floaters.Add (p.heartRate2);
		floaters.Add (p.heartRate3);
		floaters.Add (p.heartRate4);
		floaters.Add (p.heartRate5);
		floaters.Add (p.MAP1);
		floaters.Add (p.MAP2);
		floaters.Add (p.MAP3);
		floaters.Add (p.MAP4);
		floaters.Add (p.MAP5);
		floaters.Add (p.MAP);
		floaters.Add (p.pacingThreshold);
		floaters.Add (p.atropineResponsiveness);
		floaters.Add (p.nebResponsiveness);
		floaters.Add (p.fluidResponsiveness);
		floaters.Add (p.pH);
		floaters.Add (p.pO2);
		floaters.Add (p.pCO2);
		floaters.Add (p.HCO3);
		floaters.Add (p.lactate);
		floaters.Add (p.K);
		floaters.Add (p.hB);
		floaters.Add (p.glucose);
		floaters.Add (p.temperature);

		pC.floaters = floaters;

		List<int> inters = new List <int> ();
		inters.Add (p.fluidRefractoriness);
		inters.Add (p.sats);
		inters.Add (p.oxygenResponse);
		inters.Add (p.respRate);
		inters.Add ((int)p.exposureFindings);

		pC.inters = inters;

		Saver ();
	}

	private void Saver () {
		if (System.IO.File.Exists ("SOMENAMEHERE.xml")) {
			System.IO.File.Delete ("SOMENAMEHERE.xml");
		}
		FileStream fs = new FileStream("SOMENAMEHERE.xml", FileMode.OpenOrCreate);
		XmlSerializer xs = new XmlSerializer(pC.GetType());
		xs.Serialize(fs, pC);
		//fs.Close();
		fs.Dispose();
        Debug.Log("Saved!");
	}

	public void Load() {
		pC = new PatientContainer ();
		XmlSerializer xs = new XmlSerializer (typeof(PatientContainer));
		FileStream fs = new FileStream ("SOMENAMEHERE.xml", FileMode.Open);
		pC = ((PatientContainer)xs.Deserialize (fs));
		//fs.Close ();
		fs.Dispose ();
		Loader (pC);
	}

    public void LoadHub()
    {
        Debug.Log("Serializing...");
        source = "Hub";
        pC = new PatientContainer();
        XmlSerializer xs = new XmlSerializer(typeof(PatientContainer));
        FileStream fs = new FileStream("SOMENAMEHERE.xml", FileMode.Open);
        pC = ((PatientContainer)xs.Deserialize(fs));
        //fs.Close ();
        fs.Dispose();
        Debug.Log("Done serializing");
        Loader(pC);
    }

    private void Loader(PatientContainer pC) {
        Debug.Log("Deserializing");
        Patient p = new Patient();
        
        p.arrest = pC.boolers[0];
        p.conscious = pC.boolers[1];
        p.airwayObstructed = pC.boolers[2];
        p.apnoeic = pC.boolers[3];
        p.pulmonaryOedema = pC.boolers[4];
        p.bradyCardia = pC.boolers[5];

        p.diagnosis = pC.stringers[0];
        p.leadIn = pC.stringers[1];
        p.rhythm1 = pC.stringers[2];
        p.rhythm2 = pC.stringers[3];
        p.rhythm3 = pC.stringers[4];
        p.rhythm4 = pC.stringers[5];
        p.rhythm5 = pC.stringers[6];
        p.airwayObstruction = pC.stringers[7];
        p.chestFindings = pC.stringers[8];
        p.pupilsFindings = pC.stringers[9];
        p.AVPU = pC.stringers[10];

        /*int i = 0;
        foreach (float f in pC.floaters) {
            Debug.Log("Floater " + i + " " + f);
            i++;
        }*/
        p.heartRate1 = pC.floaters[0];
        p.heartRate2 = pC.floaters[1];
        p.heartRate3 = pC.floaters[2];
        p.heartRate4 = pC.floaters[3];
        p.heartRate5 = pC.floaters[4];
        p.MAP1 = pC.floaters[5];
        p.MAP2 = pC.floaters[6];
        p.MAP3 = pC.floaters[7];
        p.MAP4 = pC.floaters[8];
        p.MAP5 = pC.floaters[9];
        p.MAP = pC.floaters[10];
        p.pacingThreshold = pC.floaters[11];
        p.atropineResponsiveness = pC.floaters[12];
        p.nebResponsiveness = pC.floaters[13];
        p.fluidResponsiveness = pC.floaters[14];
        p.pH = pC.floaters[15];
        p.pO2 = pC.floaters[16];
        p.pCO2 = pC.floaters[17];
        p.HCO3 = pC.floaters[18];
        p.lactate = pC.floaters[19];
        p.K = pC.floaters[20];
        p.hB = pC.floaters[21];
        p.glucose = pC.floaters[22];
        p.temperature = pC.floaters[23];

        p.fluidRefractoriness = pC.inters[0];
        p.sats = pC.inters[1];
        p.oxygenResponse = pC.inters[2];
        p.respRate = pC.inters[3];
		p.exposureFindings = (Insights)pC.inters[4];

        /*i = 0;
        foreach (int f in pC.inters)
        {
            Debug.Log("Inter " + i + " " + f);
            i++;
        }*/
        Debug.Log("Done deserializing");
        if (source == "") {
            PatientMaker pM = FindObjectOfType<PatientMaker>();
            pM.Load(p);
        } else if (source == "Hub")
        {
            Hub hub = FindObjectOfType<Hub>();
            Debug.Log("Returning to hub");
            hub.LoadPatient(p);
        }

	}
}

[XmlRoot("PatientContainer")]
public class PatientContainer {
	[XmlArray("Boolers")]
	[XmlArrayItem("Booler")]
	public List<bool> boolers = new List <bool>();

	[XmlArray("Stringers")]
	[XmlArrayItem("Stringer")]
	public List<string> stringers = new List <string> ();

	[XmlArray("Floaters")]
	[XmlArrayItem("Floater")]
	public List<float> floaters = new List <float> ();

	[XmlArray("Inters")]
	[XmlArrayItem("Inter")]
	public List<int> inters = new List <int> ();
}