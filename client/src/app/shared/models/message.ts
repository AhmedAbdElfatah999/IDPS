export interface Message {
  id: number;
  senderId: string;
  patientName: string;
  patientPhotoUrl: string;
  recipientId: string;
  doctorName: string;
  doctorPhotoUrl: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
  doctorId:string;
  patientId:string;
}
