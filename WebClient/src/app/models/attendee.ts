export default interface Attendee {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string
  following: boolean
  followersCount: number
  followingCount: number
}
