export default interface User {
  username: string;
  displayName: string;
  image?: string;
  token: string;
}

export interface UserFormValues {
  email: string;
  password: string;
  username?: string;
  displayName?: string;
}
