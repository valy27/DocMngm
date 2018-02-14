export interface CreateDocumentModel {
    description: string;
}

export interface DocumentModel {
    id: number;
    description: string;
    name: string;
    created: Date;
    fileSize: number;
    ownerUserName: string;
}
