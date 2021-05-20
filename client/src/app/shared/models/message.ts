export interface Message {
  id: number;
  senderId: number;
  patientName: string;
  patientPhotoUrl: string;
  recipientId: number;
  doctorName: string;
  doctorPhotoUrl: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}
