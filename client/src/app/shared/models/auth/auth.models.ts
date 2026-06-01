export interface Login {
    email:string;
    password:string;
}

export interface Register extends Login {
    userName:string;
    displayName:string;
    firstName:string;
    middleName:string;
    lastName:string;
}

export class ActivateAccount {
    email:string;
    token:string;
}

export interface ResetPassword extends Login {
    token:string;
}