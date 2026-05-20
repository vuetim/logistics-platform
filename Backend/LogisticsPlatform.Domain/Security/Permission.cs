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
    User_AssignRole,
    User_Disable,
    // CUSTOMERS
    Customer_View,
    Customer_Create,
    Customer_Update,
    Customer_Delete,

    // TRACKING / MAPS
    Load_Tracking_View,
    Load_Tracking_Update,

    // FINANCIALS
    Financial_Invoice_UpdateStatus,
    Financial_Invoice_RecordPayment,
    Financial_Settlement_UpdateStatus,
    Financial_Settlement_RecordPayment,

    // CARRIER OFFERS / TENDERS
    CarrierOffer_View,
    CarrierOffer_View_All,
    CarrierOffer_Create,
    CarrierOffer_Accept,
    CarrierOffer_Reject,

    // OPERATIONAL EXCEPTIONS
    LoadException_View,
    LoadException_Create,
    LoadException_Update,

    // STOP SERVICES
    LoadStopService_View,
    LoadStopService_Create,
    LoadStopService_Delete,

    // COMPLETED LOAD CORRECTIONS
    Load_CompletedCorrection,

    // COSTS / BILLING VIEW AND EDIT
    OrderCost_View,
    OrderCost_Update,
    LoadCost_View,
    LoadCost_Update,
    Financial_View,

    // LOAD OPERATIONAL DETAILS
    Load_Operational_Update,
}

