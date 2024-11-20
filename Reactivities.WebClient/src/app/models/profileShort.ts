import User from './user'

export interface ProfileShort {
  username: string
  displayName: string
  bio?: string
  profilePhotoUrl?: string
  following: boolean
  followersCount: number
  followingCount: number
}

export class ProfileShort implements ProfileShort {
  constructor(user: User) {
    this.username = user.username
    this.displayName = user.displayName
    this.profilePhotoUrl = user.profilePhotoUrl
  }
}
