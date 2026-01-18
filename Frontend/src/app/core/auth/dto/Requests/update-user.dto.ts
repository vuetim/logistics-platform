export interface UpdateUserDto {
    fullName: string;
    email: string;
    isActive: boolean;
    newPassword?: string;
}
