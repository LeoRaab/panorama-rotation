using PanoramaRotation;

Console.Write("Specify path to pano.csv: ");
var path = Console.ReadLine();

if (path == null) return; 

var panoramaProcessor = new PanoramaProcessor();

panoramaProcessor.ProcessCsv(path);