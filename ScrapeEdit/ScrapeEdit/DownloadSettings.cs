namespace ScrapeEdit
{
    public static class DownloadSettings
    {
        public static bool Download_sstitle { get; set; } = true;
        public static bool Download_ss { get; set; } = false;
        public static bool Download_fanart { get; set; } = false;
        public static bool Download_video { get; set; } = false;
        public static bool Download_video_normalized { get; set; } = true;
        public static bool Download_themehs { get; set; } = false;
        public static bool Download_screenmarquee { get; set; } = true;
        public static bool Download_screenmarqueesmall { get; set; } = false;
        public static bool Downlaod_manuel { get; set; } = true;
        public static bool Download_steamgrid { get; set; } = false;
        public static bool Download_wheel { get; set; } = true;
        public static bool Download_wheel_hd { get; set; } = true;
        public static bool Download_wheel_carbon { get; set; } = false;
        public static bool Download_wheel_steel { get; set; } = false;
        public static bool Download_box_2D { get; set; } = true;
        public static bool Download_box_2D_side { get; set; } = false;
        public static bool Download_box_2D_back { get; set; } = false;
        public static bool Download_box_texture { get; set; } = false;
        public static bool Download_box_3D { get; set; } = true;
        public static bool Download_support_texture { get; set; } = false;
        public static bool Downlaod_support_2D { get; set; } = false;
        public static bool Download_bezel_16_9 { get; set; } = false;
        public static bool Download_mixrbv1 { get; set; } = true;
        public static bool Download_mixrbv2 { get; set; } = false;


        public static List<string> DownloadFilesTypes()
        {
            List<string> download_files = new List<string>();

            download_files.Add(Download_sstitle == true ? "sstitle" : "");
            download_files.Add(Download_ss == true ? "ss" : "");
            download_files.Add(Download_fanart == true ? "fanart" : "");
            download_files.Add(Download_video == true ? "video" : "");
            download_files.Add(Download_video_normalized == true ? "video-normalized" : "");
            download_files.Add(Download_themehs == true ? "themehs" : "");
            download_files.Add(Download_screenmarquee == true ? "screenmarquee" : "");
            download_files.Add(Download_screenmarqueesmall == true ? "screenmarqueesmall" : "");
            download_files.Add(Downlaod_manuel == true ? "manuel" : "");
            download_files.Add(Download_steamgrid == true ? "steamgrid" : "");
            download_files.Add(Download_wheel == true ? "wheel" : "");
            download_files.Add(Download_wheel_hd == true ? "wheel-hd" : "");
            download_files.Add(Download_wheel_carbon == true ? "wheel-carbon" : "");
            download_files.Add(Download_wheel_steel == true ? "wheel-steel" : "");
            download_files.Add(Download_box_2D == true ? "box-2D" : "");
            download_files.Add(Download_box_2D_side == true ? "box-2D-side" : "");
            download_files.Add(Download_box_2D_back == true ? "box-2D-back" : "");
            download_files.Add(Download_box_texture == true ? "box-texture" : "");
            download_files.Add(Download_box_3D == true ? "box-3D" : "");
            download_files.Add(Download_support_texture == true ? "support-texture" : "");
            download_files.Add(Downlaod_support_2D == true ? "support-2D" : "");
            download_files.Add(Download_bezel_16_9 == true ? "bezel-16-9" : "");
            download_files.Add(Download_mixrbv1 == true ? "mixrbv1" : "");
            download_files.Add(Download_mixrbv2 == true ? "mixrbv2" : "");


            //clean the list
            for (int i = 0; i < download_files.Count; i++)
            {
                if (download_files[i] == "")
                {
                    download_files.RemoveAt(i);
                    i--;
                }
            }

            return download_files;
        }

        
    
    }

    public class SerializableDownloadSettings()
    {
        public  bool Download_sstitle { get; set; } = true;
        public  bool Download_ss { get; set; } = false;
        public  bool Download_fanart { get; set; } = false;
        public  bool Download_video { get; set; } = false;
        public  bool Download_video_normalized { get; set; } = true;
        public  bool Download_themehs { get; set; } = false;
        public  bool Download_screenmarquee { get; set; } = true;
        public  bool Download_screenmarqueesmall { get; set; } = false;
        public  bool Downlaod_manuel { get; set; } = true;
        public  bool Download_steamgrid { get; set; } = false;
        public  bool Download_wheel { get; set; } = true;
        public bool Download_wheel_hd { get; set; } = true;
        public  bool Download_wheel_carbon { get; set; } = false;
        public  bool Download_wheel_steel { get; set; } = false;
        public  bool Download_box_2D { get; set; } = true;
        public  bool Download_box_2D_side { get; set; } = false;
        public  bool Download_box_2D_back { get; set; } = false;
        public  bool Download_box_texture { get; set; } = false;
        public  bool Download_box_3D { get; set; } = true;
        public  bool Download_support_texture { get; set; } = false;
        public  bool Downlaod_support_2D { get; set; } = false;
        public  bool Download_bezel_16_9 { get; set; } = false;
        public  bool Download_mixrbv1 { get; set; } = true;
        public  bool Download_mixrbv2 { get; set; } = false;

        public void Update()
        {

            Download_sstitle = DownloadSettings.Download_sstitle;
            Download_ss = DownloadSettings.Download_ss;
            Download_fanart = DownloadSettings.Download_fanart;
            Download_video = DownloadSettings.Download_video;
            Download_video_normalized = DownloadSettings.Download_video_normalized;
            Download_themehs = DownloadSettings.Download_themehs;
            Download_screenmarquee = DownloadSettings.Download_screenmarquee;
            Download_screenmarqueesmall = DownloadSettings.Download_screenmarqueesmall;
            Downlaod_manuel = DownloadSettings.Downlaod_manuel;
            Download_steamgrid = DownloadSettings.Download_steamgrid;
            Download_wheel = DownloadSettings.Download_wheel;
            Download_wheel_hd = DownloadSettings.Download_wheel_hd;
            Download_wheel_carbon = DownloadSettings.Download_wheel_carbon;
            Download_wheel_steel = DownloadSettings.Download_wheel_steel;
            Download_box_2D = DownloadSettings.Download_box_2D;
            Download_box_2D_side = DownloadSettings.Download_box_2D_side;
            Download_box_2D_back = DownloadSettings.Download_box_2D_back;
            Download_box_texture = DownloadSettings.Download_box_texture;
            Download_box_3D = DownloadSettings.Download_box_3D;
            Download_support_texture = DownloadSettings.Download_support_texture;
            Downlaod_support_2D = DownloadSettings.Downlaod_support_2D;
            Download_bezel_16_9 = DownloadSettings.Download_bezel_16_9;
            Download_mixrbv1 = DownloadSettings.Download_mixrbv1;
            Download_mixrbv2 = DownloadSettings.Download_mixrbv2;
            
        }

    }
}
