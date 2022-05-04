// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net;


//Array with link from toonily to search
string[] urls = {
"https://toonily.com/webtoon/overgeared/"
};

//Lista para os chapters já lidos
List<string> readed;
readed = getChaps("Read.txt");

//Lista para os links
List<string> Links = new List<string> {};

//Lista para as datas de release
List<string> Release = new List<string> { };

deleteFile("Links.txt");

//Obter os links e as releases para as listas
foreach (string url in urls)
{
    getCode(url, Links, Release);
}

//Comprar os numeros dos links com os ja lidos a ver se há novo
newRelease(Links, readed);

for (int i = 0; i < Links.Count; i++)
{
    //Console.WriteLine(Links[i]);
    //Console.WriteLine(Release[i]);
    fileWrite(Links[i], "Release: " + Release[i]);
}

//Guardar os dados no ficheiro
//PRECISA DE ALTERACOES NO GUARDAR O DADOS NO FICHEIRO
//fileWrite(link, "Release: " + release);

Process.Start("notepad.exe","Links.txt");

//Guarda os dados nas listas
void getCode(string url, List<String> Links, List<String> Release)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    StreamReader sr = new StreamReader(response.GetResponseStream());
    string data = sr.ReadToEnd();
    sr.Close();

    //Console.WriteLine(data);

    string strStart = "<h2 class=\"h4\">CHAPTERS</h2>";
    string strEnd = "</i> </span>";

    int Start, End;
    Start = data.IndexOf(strStart, 0) + strStart.Length;
    End = data.IndexOf(strEnd, Start);
    string util = data.Substring(Start, (End+4) - Start);

    //Console.WriteLine(util);
    //string link = getLink(util);
    Links.Add(getLink(util));

    //string release = getRelease(util);
    Release.Add(getRelease(util));
    
    //Console.WriteLine("Link: " + link);
    //Console.WriteLine("Release: " + release);
    //fileWrite(link, "Release: " + release);
}

//Vai buscar o link ao codigo fonte
string getLink(string data)
{
    string link = "";

    string strStart = "https";
    string strEnd = "\">";
    int Start, End;
    Start = data.IndexOf(strStart, 0);
    End = data.IndexOf(strEnd, Start);
    link = data.Substring(Start, End - Start);

    //Console.WriteLine(link);
    return link;
}

//Vai buscar a release date ao codigo fonte
string getRelease(string data)
{
    string release = "";

    string strStart = "upicn\">";
    string strEnd = "</span>";
    int Start, End;
    Start = data.IndexOf(strStart, 0) + strStart.Length;
    End = data.IndexOf(strEnd, Start);
    if(End < 0)
    {
        strStart = "<i>";
        strEnd = "</i>";
        Start = data.IndexOf(strStart, 0) + strStart.Length;
        End = data.IndexOf(strEnd, Start);
        release = data.Substring(Start, End - Start);
    }
    else
    {
        release = data.Substring(Start, End - Start);
    }

    //Console.WriteLine(release);
    return release;
}

//Guarda os dados no ficheiro
static async void fileWrite(string link, string release)
{
    using StreamWriter file = new("Links.txt", append: true);
    if (link.Contains("NEW"))
    {
        await file.WriteLineAsync(link);
        await file.WriteLineAsync(release);
    }
}

//Funcao para dar delete ao ficheiro dos links ao correr o programa para que nao haja confusao com o ficheiro que ja existia
static void deleteFile(string name)
{
    File.Delete(name);
}

//Funcao para obter os capitulos ja lidos do ficheiro
static List<string> getChaps(string fileName)
{
    List<String> chaps = new List<string>();

    using (var file = new StreamReader(fileName))
    {
        string line;
        while ((line = file.ReadLine()) != null)
        {
            chaps.Add(line);
            //Console.WriteLine(line);
        }
    }
    return chaps;
}

//Funcao para comprar os capitulos ja lidos com o link
static void newRelease(List<String> Links, List<String> readed)
{ 
    for(int i = 0; i < Links.Count; i++)
    {
        if(!Links[i].Contains(readed[i]))
            Links[i] = Links[i] + " NEW";
    }
}