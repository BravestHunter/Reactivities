import ActivityDto from './dtos/activityDto'
import User from './user'
import ProfileShort from './profileShort'

export interface Activity {
  id: number
  title: string
  date: Date
  description?: string
  category: string
  city: string
  venue: string
  isCancelled: boolean
  host: ProfileShort
  attendees: ProfileShort[]

  isHost: boolean
  isGoing: boolean
}

export class Activity implements Activity {
  constructor(init: ActivityDto, user: User | null) {
    Object.assign(this, init)

    if (user) {
      this.isGoing = this.attendees!.some((a) => a.username === user.username)
      this.isHost = this.host.username === user.username
    }

    this.date = new Date(this.date!)
  }
}
