using System;
using System.Collections;
using System.IO;
using Huawei.Agconnect.Auth;
using Huawei.Agconnect.CloudStorage;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    [SerializeField] private RawImage image;

    [SerializeField] private Text sonucTxt;
    [SerializeField] private GameObject scroll;
    private string path;
    private string writepath;

    private static bool uploaded = false;
    private static string uploadedResult = "";

    private static byte[] filedata;

    private void Start()
    {
        
        ListAllFiles();
        FileBrowser.SetFilters( true, new FileBrowser.Filter( "Images", ".jpg", ".png" ), new FileBrowser.Filter( "Text Files", ".txt", ".pdf" ) );

        // Set default filter that is selected when the dialog is shown (optional)
        // Returns true if the default filter is set successfully
        // In this case, set Images filter as the default filter
        FileBrowser.SetDefaultFilter( ".jpg" );

        // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
        // Note that when you use this function, .lnk and .tmp extensions will no longer be
        // excluded unless you explicitly add them as parameters to the function
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

        // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
        // It is sufficient to add a quick link just once
        // Name: Users
        // Path: C:\Users
        // Icon: default (folder icon)
        FileBrowser.AddQuickLink( "Users", "C:\\Users", null );
    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for( int i = 0; i < FileBrowser.Result.Length; i++ )
                Debug.Log( FileBrowser.Result[i] );

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( FileBrowser.Result[0] );

            var storageManagement = StorageManagement.GetInstance();
            var storageReference = storageManagement.GetStorageReference(FileBrowser.Result[0]);
            var uploadTask = storageReference.PutBytes(bytes);
            uploadTask.AddListener(new UploadResultListener());
        }
    }

    public void Exit()
    {
        if (AGConnectAuth.Instance.GetCurrentUser()!=null)
        {
            AGConnectAuth.Instance.SignOut();
        }
        SceneManager.LoadScene("LoginScene");
    }

    public void DownloadPoster()
    {
        path = Application.persistentDataPath + "/Poster.png";
        var storageManagement = StorageManagement.GetInstance();
        var storageReference = storageManagement.GetStorageReference("/Poster.png");
        var downloadTask = storageReference.GetFile(new FileInfo(path));
        downloadTask.AddListener(new DownloadResultListener(path));
    }

    public async void DeleteGameFile()
    {
        var storageManagement = StorageManagement.GetInstance();
        var storageReference = storageManagement.GetStorageReference("/oyundosya.txt");
        await storageReference.DeleteAsync();
    }

    public void Update()
    {
        if (filedata != null)
        {
            var _texture = new Texture2D(1, 1);
            _texture.LoadImage(filedata);
            image.texture = _texture;
            filedata = null;
        }

        if (uploaded)
        {
            sonucTxt.text = uploadedResult;
            uploaded = false;
        }
    }

    private async void ListAllFiles()
    {
        var storageManagement = StorageManagement.GetInstance();
        var storageReference = storageManagement.GetStorageReference();
        var result = await storageReference.ListAllAsync();
        GameObject baseobj = new GameObject();
        var text = baseobj.AddComponent<Text>();
        text.supportRichText = true;
        text.resizeTextForBestFit = true;
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 32);
        var i = 0;
        foreach (var file in result.Files)
        {
            if (i==0)
            {
                path = Application.persistentDataPath + "/unity.pptx";
                var downloadTask = file.GetFile(new FileInfo(path));
                downloadTask.AddListener(new DownloadResultListener(path));
            }
            GameObject o = Instantiate(baseobj, scroll.transform);
            o.GetComponent<Text>().text = file.GetName();
            i++;
        }
        
    }
    

    public void UpdateGameFile()
    {
        StartCoroutine( ShowLoadDialogCoroutine() );
        
        
        // if (!Directory.Exists(Application.persistentDataPath + "/GameFiles/"))
        // {
        //     Directory.CreateDirectory(Application.persistentDataPath + "/GameFiles/");
        // }
        //
        // writepath = Application.persistentDataPath + "/GameFiles/dosya.txt";
        // File.WriteAllText(writepath, "Kızanı koydum bonbon almaya");
        // var storageManagement = StorageManagement.GetInstance();
        // var storageReference = storageManagement.GetStorageReference("/oyundosya.txt");
        // var uploadTask = storageReference.PutFile(new FileInfo(writepath));
        // uploadTask.AddListener(new UploadResultListener());
    }

    private class UploadResultListener : IOnResultListener<UploadTask.UploadResult>
    {
        public void OnSuccess(UploadTask.UploadResult result)
        {
            Console.WriteLine("Upload completed. Uploaded byte: " + result.GetBytesTransferred() + "Total byte: " +
                              result.GetTotalByteCount());
            uploaded = true;
            uploadedResult = "Upload completed. Uploaded byte: " + result.GetBytesTransferred() + "Total byte: " +
                             result.GetTotalByteCount();
        }

        public void OnProgress(UploadTask.UploadResult result)
        {
            Console.WriteLine("Uploaded byte: " + result.GetBytesTransferred());
            Console.WriteLine("Total byte: " + result.GetTotalByteCount());
        }

        public void OnFailure(Exception exception)
        {
            Console.WriteLine($"Upload task failed: {exception.Message}");
            uploaded = true;
            uploadedResult = $"Upload task failed: {exception.Message}";
        }

        public void OnCanceled()
        {
            Console.WriteLine("Upload task canceled.");
        }
    }


    class DownloadResultListener : IOnResultListener<DownloadTask.DownloadResult>
    {
        private string _path;

        public DownloadResultListener(string path)
        {
            _path = path;
        }

        public void OnSuccess(DownloadTask.DownloadResult result)
        {
            Console.WriteLine("Download completed. Downloaded byte: " + result.GetBytesTransferred() + "Total byte: " +
                              result.GetTotalByteCount());
            filedata = File.ReadAllBytes(_path);
        }

        public void OnProgress(DownloadTask.DownloadResult result)
        {
            Console.WriteLine("Downloaded byte: " + result.GetBytesTransferred());
            Console.WriteLine("Total byte: " + result.GetTotalByteCount());
        }

        public void OnFailure(Exception exception)
        {
            Console.WriteLine($"Download task failed: {exception.Message}");
        }

        public void OnCanceled()
        {
            Console.WriteLine("Download task canceled.");
        }
    }
}