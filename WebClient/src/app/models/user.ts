export default interface User {
  username: string
  displayName: string
  profilePhotoUrl?: string
}

export interface AccessToken {
  accessToken: string
}

export interface UserFormValues {
  email: string
  password: string
  username?: string
  displayName?: string
}
