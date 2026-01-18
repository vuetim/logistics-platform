export enum Permission {
    Load_View = 'Load_View',
    Load_Create = 'Load_Create',
    Load_Update = 'Load_Update',
    Load_ChangeStatus = 'Load_ChangeStatus',
    Load_Archive = 'Load_Archive',

    LoadNote_View = 'LoadNote_View',
    LoadNote_Create_Internal = 'LoadNote_Create_Internal',
    LoadNote_Create_Public = 'LoadNote_Create_Public',

    LoadDocument_View = 'LoadDocument_View',
    LoadDocument_Upload = 'LoadDocument_Upload',
    LoadDocument_Delete = 'LoadDocument_Delete',

    User_View_All = 'User_View_All',
    User_View_Self = 'User_View_Self',
    User_Update = 'User_Update',
    User_Delete = 'User_Delete',
    User_AssignRole = 'User_AssignRole',
    User_Disable = 'User_Disable'
}
