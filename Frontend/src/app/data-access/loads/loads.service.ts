import { Injectable } from "@angular/core";
import { LoadsApi } from "./loads.api";
import { LoadsQueryParameters } from "../../core/models/loads/loads-query-parameters.model";

@Injectable({ providedIn: 'root' })
export class LoadsService {
  constructor(private api: LoadsApi) {}

  getPaged(params: LoadsQueryParameters) {
    return this.api.getPaged(params);
  }

  getDetails(id: string) {
    return this.api.getDetails(id);
  }

  update(id: string, dto: unknown) {
    return this.api.update(id, dto);
  }

  changeStatus(id: string, status: number) {
    return this.api.changeStatus(id, status);
  }

  dispatch(id: string, dto: unknown) {
    return this.api.dispatch(id, dto);
  }

  archive(id: string) {
    return this.api.archive(id);
  }

  getCosts(loadId: string) {
    return this.api.getCosts(loadId);
  }

  updateCosts(loadId: string, dto: unknown) {
    return this.api.updateCosts(loadId, dto);
  }

  getNotes(loadId: string) {
    return this.api.getNotes(loadId);
  }

  createNote(loadId: string, text: string, isInternal: boolean) {
    return this.api.createNote(loadId, { message: text, isInternal });
  }

  getDocuments(loadId: string) {
    return this.api.getDocuments(loadId);
  }

  uploadDocument(loadId: string, data: FormData) {
    return this.api.uploadDocument(loadId, data);
  }

  deleteDocument(loadId: string, documentId: string) {
    return this.api.deleteDocument(loadId, documentId);
  }

  getActivity(loadId: string) {
    return this.api.getActivity(loadId);
  }

  getInvoice(loadId: string) {
    return this.api.getInvoice(loadId);
  }

  getSettlement(loadId: string) {
    return this.api.getSettlement(loadId);
  }

  invoicePdfUrl(loadId: string, invoiceId: string) {
    return this.api.invoicePdfUrl(loadId, invoiceId);
  }

  settlementPdfUrl(loadId: string, settlementId: string) {
    return this.api.settlementPdfUrl(loadId, settlementId);
  }

  downloadInvoicePdf(loadId: string, invoiceId: string) {
    return this.api.downloadInvoicePdf(loadId, invoiceId);
  }

  downloadSettlementPdf(loadId: string, settlementId: string) {
    return this.api.downloadSettlementPdf(loadId, settlementId);
  }

  markStop(stopId: string, action: 'enroute' | 'arrive' | 'loaded' | 'unloaded') {
    return this.api.markStop(stopId, action);
  }

  createStop(loadId: string, dto: unknown) {
    return this.api.createStop(loadId, dto);
  }

  updateStop(loadId: string, stopId: string, dto: unknown) {
    return this.api.updateStop(loadId, stopId, dto);
  }

  deleteStop(loadId: string, stopId: string) {
    return this.api.deleteStop(loadId, stopId);
  }

  updateItem(loadId: string, itemId: string, dto: unknown) {
    return this.api.updateItem(loadId, itemId, dto);
  }

  deleteItem(loadId: string, itemId: string) {
    return this.api.deleteItem(loadId, itemId);
  }

  getCarrierAssignments(loadId: string) {
    return this.api.getCarrierAssignments(loadId);
  }

  tenderCarrier(loadId: string, dto: unknown) {
    return this.api.tenderCarrier(loadId, dto);
  }

  acceptCarrierAssignment(loadId: string, assignmentId: string) {
    return this.api.acceptCarrierAssignment(loadId, assignmentId);
  }

  rejectCarrierAssignment(loadId: string, assignmentId: string) {
    return this.api.rejectCarrierAssignment(loadId, assignmentId);
  }

  getOpenCarrierOffers() {
    return this.api.getOpenCarrierOffers();
  }

  getExceptions(loadId: string) {
    return this.api.getExceptions(loadId);
  }

  createException(loadId: string, dto: unknown) {
    return this.api.createException(loadId, dto);
  }

  updateException(loadId: string, exceptionId: string, dto: unknown) {
    return this.api.updateException(loadId, exceptionId, dto);
  }

  getStopServices(stopId: string) {
    return this.api.getStopServices(stopId);
  }

  createStopService(stopId: string, dto: unknown) {
    return this.api.createStopService(stopId, dto);
  }

  deleteStopService(stopId: string, serviceId: string) {
    return this.api.deleteStopService(stopId, serviceId);
  }
}
