namespace LogisticsPlatform.Domain.Security;

public enum Permission
{
    // LOAD
    Load_View,
    Load_Create,
    Load_Update,
    Load_ChangeStatus,
    Load_Archive,
    Load_Dispatch,
    Load_Tender,
    Load_AssignDelayResponsibility,
    // LOAD NOTES
    LoadNote_View,
    LoadNote_Create_Internal,
    LoadNote_Create_Public,

    // LOAD DOCUMENTS
    LoadDocument_View,
    LoadDocument_Upload,
    LoadDocument_Delete,


    //Load from order
    Load_CreateFromOrder,
        // USERS
    User_View_All,
    User_View_Self,
    User_Update,
    User_Delete,
    User_AssignRole
}
