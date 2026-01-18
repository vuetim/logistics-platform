export interface UserDto {
    id: string;
    fullName: string;
    email: string;
    roles: string[];
    isActive: boolean;
    newPassword: string;
}
