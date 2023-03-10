namespace D11
{
    public enum ImagePickerType
    {
        // choice if he user wants to choose
        CHOICE = 0,
        // Opens camera
        CAMERA = 1,
        // Opens gallery
        GALLERY = 2
    }

    //Status of a error.
    public enum ImagePickerErrorCode
    {
        ERROR_PERMISSION_FAILED = 1,
        ERROR_FILE_NOT_READABLE = 2,
        ERROR_INTERNAL_ERROR = 4,
        ERROR_FILE_CANT_CREATE = 5,
    }

    public enum UpdateMode
    {
        // Update from playstore
        PLAY_STORE = 0,
        // Update from third party website via download apk
        THIRD_PARTY = 1
    }

    public enum UpdateType
    {
        // Flexible will let the user 
        // choose if he wants to update the app
        FLEXIBLE = 0,
        // Immediate will update right away
        IMMEDIATE = 1
    }

    //Status of a download / install.
    public enum InstallStatus
    {
        DOWNLOADED = 11,
        CANCELED = 6,
        FAILED = 5,
        INSTALLED = 4,
        INSTALLING = 3,
        DOWNLOADING = 2,
        PENDING = 1,
        UNKNOWN = 0
    }

    //Status of a error.
    public enum InstallErrorCode
    {
        ERROR_DOWNLOAD_NOT_PRESENT = -7,
        ERROR_API_NOT_AVAILABLE = -3,
        ERROR_INSTALL_IN_PROGRESS = -8,
        ERROR_INSTALL_NOT_ALLOWED = -6,
        ERROR_INSTALL_UNAVAILABLE = -5,
        ERROR_INTERNAL_ERROR = -100,
        ERROR_INVALID_REQUEST = -4,
        ERROR_PLAY_STORE_NOT_FOUND = -9,
        ERROR_UNKNOWN = -2,
        NO_ERROR = 0,
        ERROR_LIBRARY = -1,
        ERROR_STORAGE_PERMISSION = -101,
        ERROR_NETWORK = -102
    }
}