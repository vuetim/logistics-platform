namespace LogisticsPlatform.Domain.Security;

public enum Permission
{
    // LOAD
    Load_View,
    Load_Create,
    Load_Update,
    Load_ChangeStatus,
    Load_Archive,

    // LOAD NOTES
    LoadNote_View,
    LoadNote_Create_Internal,
    LoadNote_Create_Public,

    // LOAD DOCUMENTS
    LoadDocument_View,
    LoadDocument_Upload,
    LoadDocument_Delete
}
