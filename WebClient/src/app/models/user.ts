export default interface User {
  username: string;
  displayName: string;
  image?: string;
  accessToken: string;
}

export interface UserFormValues {
  email: string;
  password: string;
  username?: string;
  displayName?: string;
}
