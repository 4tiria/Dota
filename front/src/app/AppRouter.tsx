import AccountFeed from 'pages/account/ui/AccountFeed'
import React from 'react'
import { Navigate, Route, Routes } from 'react-router-dom'
import Login from '../components/account/login/Login'
import Registration from '../components/account/registration/Registration'
import Settings from '../components/account/settings/Settings'
import HeroInfo from '../components/hero/HeroInfo'
import HeroList from '../components/heroList/HeroList'
import Items from '../components/item/Items'
import MatchInfo from '../components/match/MatchInfo'
import MatchList from '../components/match/list/MatchList'

const AppRouter = () => {
    return (
        <>
            <Routes>
                <Route path="*" element={<Navigate to="/heroes" replace />} />
                <Route path="/heroes" element={<HeroList />} />
                <Route path="/hero/:name" element={<HeroInfo />} />
                <Route path="/match" element={<MatchInfo />} />
                <Route path="/matches" element={<MatchList />} />
                <Route path="/items" element={<Items />} />
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Registration />} />
                <Route path="/settings" element={<Settings />} />
                <Route path="/accounts/feed" element={<AccountFeed />} />
            </Routes>
        </>
    )
}

export default AppRouter
