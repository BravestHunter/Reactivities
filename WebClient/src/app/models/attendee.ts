export default interface Attendee {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string
  followersCount: number
  followingCount: number
  following: boolean
}
