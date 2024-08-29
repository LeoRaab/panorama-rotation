using CsvHelper.Configuration.Attributes;

namespace PanoramaRotation;

public class PanoramaData
{
    [Name("# pano poses v1.0: ID")]
    public int id { get; set; }

    [Name("filename")]
    public string fileName { get; set; }
    
    [Name("pano_pos_x")]
    public string posX { get; set; }
    
    [Name("pano_pos_y")]
    public string posY { get; set; }
    
    [Name("pano_pos_z")]
    public string posZ { get; set; }
    
    [Name("pano_ori_w")]
    public string orientationW { get; set; }
    
    [Name("pano_ori_x")]
    public string orientationX { get; set; }
    
    [Name("pano_ori_y")]
    public string orientationY { get; set; }
    
    [Name("pano_ori_z")]
    public string orientationZ { get; set; }
    
    [Optional]
    [Name("yaw_to_north")]
    public string? yawToNorth { get; set; }
}