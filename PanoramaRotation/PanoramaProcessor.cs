using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace PanoramaRotation;

public class PanoramaProcessor
{
    private string? _csvPath; 
    private List<PanoramaData>? _panoramaData;
    private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        Delimiter = "; "
    };

    public void ProcessCsv(string? path)
    {
        _csvPath = path;
        
        LoadCsv();
        
        if (_panoramaData == null) return;
        
        WriteCsv();
    }

    private void LoadCsv()
    {
        try
        {
            if (_csvPath == null)
            {
                throw new Exception("No path was specified.");
            }

            var streamReader = new StreamReader(_csvPath);
            var csvReader = new CsvReader(streamReader, _csvConfig);

            _panoramaData = csvReader.GetRecords<PanoramaData>().ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Loading csv file failed with following error: {e.Message}");
        }
    }

    private void WriteCsv()
    {
        try
        {
            if (_csvPath == null)
            {
                throw new Exception("No path was specified.");
            }

            if (_panoramaData == null)
            {
                throw new Exception("No panorama data was found.");
            }

            foreach (var pano in _panoramaData)
            {
                var yaw = Utils.CalculateYawToNorth(pano.orientationX, pano.orientationY, pano.orientationZ, pano.orientationW);
                
                pano.yawToNorth = yaw?.ToString(CultureInfo.InvariantCulture);
            }

            try
            {
                var filePath =
                    $"{Path.GetDirectoryName(_csvPath)}\\{Path.GetFileNameWithoutExtension(_csvPath)}_yaw.csv";
                var streamWriter = new StreamWriter(filePath);
                using var csvWriter = new CsvWriter(streamWriter, _csvConfig);
                csvWriter.WriteRecords(_panoramaData);
            }
            catch (Exception e)
            {            
                Console.WriteLine($"Writing csv file failed with following error: {e.Message}");
            }
            finally
            {
                Console.WriteLine($"Rotation of {_panoramaData.Count} panorama(s) successfully processed!");
            }
            
            
        }
        catch (Exception e)
        {
            Console.WriteLine($"Writing csv file failed with following error: {e.Message}");
        }
    }
}