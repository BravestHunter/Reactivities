export default interface ChatComment {
  id: number
  body: string
  createdAt: Date
  username: string
  displayName: string
  profilePhotoUrl?: string
}
