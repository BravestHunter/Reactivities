import Attendee from '../../../models/attendee'

export default interface ActivityDto {
  id: number
  title: string
  date: Date
  description?: string
  category: string
  city: string
  venue: string
  isCancelled: boolean
  host: Attendee
  attendees: Attendee[]
}
