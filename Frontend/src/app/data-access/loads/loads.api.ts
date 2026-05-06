import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../../core/config/endpoints";
import { PagedResult } from "../../core/models/pagination/paged-result.model";
import { CarrierSettlementDto, CustomerInvoiceDto, LoadActivityDto, LoadCostDto, LoadDetailsDto, LoadDocumentDto, LoadNoteDto } from "../../core/models/loads/load-details.dto";
import { LoadListItem } from "../../core/models/loads/load-list-item.model";
import { LoadsQueryParameters } from "../../core/models/loads/loads-query-parameters.model";

@Injectable({ providedIn: 'root' })
export class LoadsApi {
  private readonly baseUrl = API_ENDPOINTS.loads;

  constructor(private http: HttpClient) {}

  getPaged(params: LoadsQueryParameters) {
    return this.http.get<PagedResult<LoadListItem>>(this.baseUrl, { params: params as any });
  }

  getDetails(id: string) {
    return this.http.get<LoadDetailsDto>(`${this.baseUrl}/${id}`);
  }

  update(id: string, dto: unknown) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  changeStatus(id: string, status: number) {
    return this.http.patch<void>(`${this.baseUrl}/${id}/status`, null, {
      params: { status }
    });
  }

  dispatch(id: string, dto: unknown) {
    return this.http.post<void>(`${this.baseUrl}/${id}/dispatch`, dto);
  }

  archive(id: string) {
    return this.http.patch<void>(`${this.baseUrl}/${id}/archive`, {});
  }

  getCosts(loadId: string) {
    return this.http.get<LoadCostDto>(`${API_ENDPOINTS.loadCosts}/${loadId}/costs`);
  }

  updateCosts(loadId: string, dto: unknown) {
    return this.http.put<void>(`${API_ENDPOINTS.loadCosts}/${loadId}/costs`, dto);
  }

  getNotes(loadId: string) {
    return this.http.get<LoadNoteDto[]>(`${API_ENDPOINTS.loadNotes}/${loadId}/notes`);
  }

  createNote(loadId: string, dto: { message: string; isInternal: boolean }) {
    return this.http.post<void>(`${API_ENDPOINTS.loadNotes}/${loadId}/notes`, dto);
  }

  getDocuments(loadId: string) {
    return this.http.get<LoadDocumentDto[]>(`${API_ENDPOINTS.loadDocuments}/${loadId}/documents`);
  }

  uploadDocument(loadId: string, data: FormData) {
    return this.http.post<void>(`${API_ENDPOINTS.loadDocuments}/${loadId}/documents/upload`, data);
  }

  deleteDocument(loadId: string, documentId: string) {
    return this.http.delete<void>(`${API_ENDPOINTS.loadDocuments}/${loadId}/documents/${documentId}`);
  }

  getActivity(loadId: string) {
    return this.http.get<LoadActivityDto[]>(`${this.baseUrl}/${loadId}/activity`);
  }

  getInvoice(loadId: string) {
    return this.http.get<CustomerInvoiceDto>(`${this.baseUrl}/${loadId}/financials/invoices`);
  }

  getSettlement(loadId: string) {
    return this.http.get<CarrierSettlementDto>(`${this.baseUrl}/${loadId}/financials/settlements`);
  }

  invoicePdfUrl(loadId: string, invoiceId: string) {
    return `${this.baseUrl}/${loadId}/financials/invoices/${invoiceId}/pdf`;
  }

  settlementPdfUrl(loadId: string, settlementId: string) {
    return `${this.baseUrl}/${loadId}/financials/settlements/${settlementId}/pdf`;
  }

  downloadInvoicePdf(loadId: string, invoiceId: string) {
    return this.http.get(`${this.baseUrl}/${loadId}/financials/invoices/${invoiceId}/pdf`, {
      responseType: 'blob'
    });
  }

  downloadSettlementPdf(loadId: string, settlementId: string) {
    return this.http.get(`${this.baseUrl}/${loadId}/financials/settlements/${settlementId}/pdf`, {
      responseType: 'blob'
    });
  }

  markStop(stopId: string, action: 'enroute' | 'arrive' | 'loaded' | 'unloaded') {
    return this.http.post<void>(`${API_ENDPOINTS.loadStopExecution}/${stopId}/${action}`, {});
  }

  createStop(loadId: string, dto: unknown) {
    return this.http.post<void>(`${API_ENDPOINTS.loadStops}/${loadId}/stops`, dto);
  }

  updateStop(loadId: string, stopId: string, dto: unknown) {
    return this.http.put<void>(`${API_ENDPOINTS.loadStops}/${loadId}/stops/${stopId}`, dto);
  }

  deleteStop(loadId: string, stopId: string) {
    return this.http.delete<void>(`${API_ENDPOINTS.loadStops}/${loadId}/stops/${stopId}`);
  }
}
